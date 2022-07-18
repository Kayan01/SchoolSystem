using Auth.Core.Interfaces;
using Auth.Core.Models;
using Auth.Core.Models.Alumni;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.Alumni;
using Auth.Core.ViewModels.Promotion;
using IPagedList;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Enums;
using Shared.Pagination;
using Shared.PubSub;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IRepository<SchoolClass, long> _schoolClassRepo;
        private readonly IRepository<PromotionLog, long> _promotionLogRepo;
        private readonly IRepository<Alumni, long> _alumniRepo;
        private readonly IAlumniService _alumniService;
        private readonly IPublishService _publishService;
        private readonly IRepository<School, long> _schoolRepo;

        public PromotionService(
            IUnitOfWork unitOfWork,
            IRepository<Student, long> studentRepo,
            IRepository<SchoolClass, long> schoolClassRepo,
            IRepository<PromotionLog, long> promotionLogRepo,
            IRepository<Alumni, long> alumniRepo,
            IAlumniService alumniService,
            IPublishService publishService,
            IRepository<School, long> schoolRepo
            )
        {
            _unitOfWork = unitOfWork;
            _schoolClassRepo = schoolClassRepo;
            _studentRepo = studentRepo;
            _promotionLogRepo = promotionLogRepo;
            _alumniRepo = alumniRepo;
            _alumniService = alumniService;
            _publishService = publishService;
            _schoolRepo = schoolRepo;
        }

        public async Task<ResultModel<string>> PromoteAllStudent(PromotionSharedModel model)
        {
            Random r = new Random();
            //get all classes in school
            var classes = await _schoolClassRepo.GetAll().Where(m => m.TenantId == model.TenantId).OrderBy(m => m.Sequence).AsNoTracking().ToListAsync();
            if (classes == null)
            {
                throw new Exception("Classes have not been setup");
            }
            if (classes.Any(m => m.Sequence == 0))
            {
                throw new Exception("One or more Promotion Sequence have not been setup");
            }

            var students = await _studentRepo.GetAll().Where(m => m.TenantId == model.TenantId).ToListAsync();
            if (students == null)
            {
                throw new Exception("Students have not been setup");
            }

            var publishObj = new List<StudentSharedModel>();

            var repeatingStudentIds = model.StudentPromotionInfoList.Where(m => !m.PassedCutoff).Select(m => m.StudentId).ToList();

            var previousRepeats = await _promotionLogRepo.GetAll().Where(m =>
                repeatingStudentIds.Contains(m.StudentId.Value) &&
                m.PromotionStatus == Enumeration.PromotionStatus.Repeated
            ).ToListAsync();


            foreach (var studentItem in model.StudentPromotionInfoList)
            {
                var student = students.FirstOrDefault(m => m.Id == studentItem.StudentId);

                if (student == null)
                {
                    continue;
                }

                if (model.IsAutomaticPromotion) // Automatic promotion
                {
                    if (studentItem.PassedCutoff) // if student passed cutoff mark
                    {
                        var curclass = classes.SingleOrDefault(m => m.Id == student.ClassId); //get current class

                        if (curclass == null) //Make sure class is valid
                        {
                            throw new Exception($"Current Class was not found for student {student.Id}");
                        }

                        if (curclass.IsTerminalClass)
                        {
                            _alumniRepo.Insert(new Alumni(student, model.SessionName,"Promotion")); //if current class is a terminal class, create an alumni record.

                            try
                            {
                                _unitOfWork.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                                throw new DbUpdateException(ex.Message);
                            }
                        }
                        
                        //Promote Student

                        // Update this to tackle classpool.
                        var nextClasses = classes.Where(m => m.Sequence == curclass.Sequence + 1).ToList();
                        if (nextClasses == null || nextClasses.Count <= 0)
                        {
                            _promotionLogRepo.Insert(new PromotionLog()
                            {
                                PromotionStatus = Enumeration.PromotionStatus.Graduated,
                                SessionSetupId = model.SessionId,
                                FromClassId = student.ClassId,
                                ToClassId = null,
                                StudentId = student.Id,
                                AverageScore = studentItem.Average,
                                TenantId = model.TenantId
                            });

                            student.ClassId = null;
                            student.StudentStatusInSchool = StudentStatusInSchool.IsGraduated;
                        }
                        else
                        {
                            var nextClass = nextClasses.FirstOrDefault(m => m.ClassArm == curclass.ClassArm) ?? nextClasses[r.Next(0, nextClasses.Count)];

                            _promotionLogRepo.Insert(new PromotionLog()
                            {
                                PromotionStatus = Enumeration.PromotionStatus.Promoted,
                                SessionSetupId = model.SessionId,
                                SessionName = model.SessionName,
                                FromClassId = student.ClassId,
                                ToClassId = nextClass.Id,
                                StudentId = student.Id,
                                AverageScore = studentItem.Average,
                                TenantId = model.TenantId
                            });

                            student.ClassId = nextClass.Id;
                        }
                    }
                    else // student did not meet cutoff mark
                    {
                        var previousRepeat = previousRepeats.Where(m => m.StudentId == student.Id && m.ToClassId == student.ClassId).ToList();

                        if (previousRepeat.Count >= model.MaxRepeats) // has reached max number of repeats
                        {
                            // add repetition promotion log, remove student from class and set student status to withdrawn
                            _promotionLogRepo.Insert(new PromotionLog()
                            {
                                PromotionStatus = Enumeration.PromotionStatus.Withdrawn,
                                SessionSetupId = model.SessionId,
                                SessionName = model.SessionName,
                                FromClassId = student.ClassId,
                                WithdrawalReason= $"Has repeated more than {model.MaxRepeats} times",
                                ToClassId = null,
                                StudentId = student.Id,
                                TenantId = model.TenantId
                            });

                            student.StudentStatusInSchool = StudentStatusInSchool.IsWithdrawn;
                            student.ClassId = null;
                        }
                        else // student has not reached max number of repeats
                        {
                            // add repetition promotion log
                            _promotionLogRepo.Insert(new PromotionLog()
                            {
                                PromotionStatus = Enumeration.PromotionStatus.Repeated,
                                SessionSetupId = model.SessionId,
                                SessionName = model.SessionName,
                                FromClassId = student.ClassId,
                                ToClassId = student.ClassId,
                                StudentId = student.Id,
                                TenantId = model.TenantId
                            });
                        }
                    }

                }
                else // add to class pool
                {
                    _promotionLogRepo.Insert(new PromotionLog()
                    {
                        PromotionStatus = Enumeration.PromotionStatus.InPool,
                        SessionSetupId = model.SessionId,
                        SessionName = model.SessionName,
                        FromClassId = student.ClassId,
                        ToClassId = null,
                        StudentId = student.Id,
                        AverageScore = studentItem.Average,
                        TenantId = model.TenantId
                    });

                    student.ClassId = null;
                }

                publishObj.Add(new StudentSharedModel
                {
                    Id = student.Id,
                    RegNumber = student.RegNumber,
                    IsActive = student.IsActive,
                    ClassId = student.ClassId,
                    TenantId = student.TenantId,
                    UserId = student.UserId,
                    Sex = student.Sex,
                    DoB = student.DateOfBirth,
                    StudentStatusInSchool = student.StudentStatusInSchool,
                });
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new DbUpdateException(ex.Message);
            }
           
            // Publish updated Students.
            await _publishService.PublishMessage(Topics.Student, BusMessageTypes.STUDENT, publishObj);

            return new ResultModel<string>(data: "Students Promoted successfully");

        }

        public async Task<ResultModel<PaginatedModel<ClassPoolVM>>> GetClassPool(QueryModel vm, long sessionId, long? fromClassId)
        {
            var result = new ResultModel<PaginatedModel<ClassPoolVM>>();

            var query = _promotionLogRepo.GetAll().Where(m => m.PromotionStatus == Enumeration.PromotionStatus.InPool && m.SessionSetupId == sessionId);

            if (fromClassId != null)
            {
                query = query.Where(m => m.FromClassId == fromClassId);
            }

            var newQuery = query.OrderBy(x => x.Id)
                .Select(m => new ClassPoolVM
                {
                    Id = m.Id,
                    Average = m.AverageScore,
                    StudentName = $"{m.Student.User.FirstName} {m.Student.User.LastName}",
                    Level = m.FromClass.SchoolSection.Name,
                    PreviousClass = $"{m.FromClass.Name} {m.FromClass.ClassArm}",
                    RegNumber = m.Student.RegNumber
                });

            var pagedData = await newQuery.ToPagedListAsync(vm.PageIndex, vm.PageSize);

            result.Data = new PaginatedModel<ClassPoolVM>(pagedData, vm.PageIndex, vm.PageSize, pagedData.TotalItemCount);

            return result;
        }

        public async Task<ResultModel<string>> PostClassPool(List<ClassPoolVM> VMs)
        {
            //get all classes in school
            var classes = await _schoolClassRepo.GetAll().OrderBy(m => m.Sequence).AsNoTracking().ToListAsync();
            if (classes == null)
            {
                return new ResultModel<string>(errorMessage: "Classes have not been setup");
            }
            if (classes.Any(m => m.Sequence == 0))
            {
                return new ResultModel<string>(errorMessage: "One or more Promotion Sequence have not been setup");
            }


            var idList = VMs.Select(m => m.Id);

            var poolItems = await _promotionLogRepo.GetAll().Include(m => m.Student).Where(m => idList.Contains(m.Id)).ToListAsync();

            var publishObj = new List<StudentSharedModel>();

            foreach (var item in poolItems)
            {
                var vm = VMs.First(m => m.Id == item.Id);

                var curclass = classes.SingleOrDefault(m => m.Id == item.FromClassId); //get current class

                if (curclass == null) //Make sure class is valid
                {
                    return new ResultModel<string>(errorMessage: $"Current Class was not found for Class pool item -> {item.Id}");
                }

                if (vm.Status == ClassPoolVM.ClassPoolVM_PromotionStatus.Graduated)
                {
                    _alumniRepo.Insert(new Alumni(item.Student, item.SessionName)); //create an alumni record.

                    item.PromotionStatus = Enumeration.PromotionStatus.Graduated;
                    item.ToClassId = null;

                    item.Student.ClassId = null;
                }
                else if (vm.Status == ClassPoolVM.ClassPoolVM_PromotionStatus.Repeated)
                {
                    item.PromotionStatus = Enumeration.PromotionStatus.Repeated;

                    if (vm.ToClass == 0) //Make sure class is valid
                    {
                        item.ToClassId = item.FromClassId;
                        item.Student.ClassId = item.FromClassId;
                    }
                    else
                    {
                        item.ToClassId = vm.ToClass;
                        item.Student.ClassId = vm.ToClass;
                    }
                }
                else if (vm.Status == ClassPoolVM.ClassPoolVM_PromotionStatus.Withdrawn)
                {
                    _alumniRepo.Insert(new Alumni(item.Student, item.SessionName)); //create an alumni record.

                    item.PromotionStatus = Enumeration.PromotionStatus.Withdrawn;

                    item.Student.StudentStatusInSchool = StudentStatusInSchool.IsWithdrawn;
                    item.WithdrawalReason = vm.WithdrawalReason;
                    item.Student.ClassId = null;
                }
                else if (vm.Status == ClassPoolVM.ClassPoolVM_PromotionStatus.ReInstated)
                {
                    item.PromotionStatus = Enumeration.PromotionStatus.Re_Instated;
                    item.ReInstateReason = vm.ReInstateReason;

                    if (vm.ToClass == 0) //Make sure class is valid
                    {
                        item.ToClassId = item.FromClassId;
                        item.Student.ClassId = item.FromClassId;
                    }
                    else
                    {
                        item.ToClassId = vm.ToClass;
                        item.Student.ClassId = vm.ToClass;
                    }
                }
                else // promote student
                {

                    if (vm.ToClass == 0) //Make sure class is valid
                    {
                        return new ResultModel<string>(errorMessage: $"To Class should not be null for a promoted student");
                    }

                    if (curclass.IsTerminalClass)
                    {
                        _alumniRepo.Insert(new Alumni(item.Student, item.SessionName)); //if current class is a terminal class, create an alumni record.
                    }


                    var nextClass = classes.SingleOrDefault(m => m.Id == vm.ToClass); //get current class

                    item.PromotionStatus = Enumeration.PromotionStatus.Promoted;
                    item.ToClassId = nextClass.Id;

                    item.Student.ClassId = nextClass.Id;
                }



                publishObj.Add(new StudentSharedModel
                {
                    Id = item.Student.Id,
                    RegNumber = item.Student.RegNumber,
                    IsActive = item.Student.IsActive,
                    ClassId = item.Student.ClassId,
                    TenantId = item.Student.TenantId,
                    UserId = item.Student.UserId,
                    Sex = item.Student.Sex,
                    DoB = item.Student.DateOfBirth,
                    StudentStatusInSchool = item.Student.StudentStatusInSchool,
                });
            }

            _unitOfWork.SaveChanges();

            // Publish updated Students.
            await _publishService.PublishMessage(Topics.Student, BusMessageTypes.STUDENT, publishObj);

            return new ResultModel<string>(data: "Updated successfully");
        }

        public async Task<ResultModel<PaginatedModel<ClassPoolVM>>> GetWithdrawnList(QueryModel vm, long sessionId, long? fromClassId)
        {
            var result = new ResultModel<PaginatedModel<ClassPoolVM>>();

            var query = _promotionLogRepo.GetAll().Where(m => m.PromotionStatus == Enumeration.PromotionStatus.Withdrawn && m.SessionSetupId == sessionId);

            if (fromClassId != null)
            {
                query = query.Where(m => m.FromClassId == fromClassId);
            }

            var newQuery = query.OrderBy(x => x.Id)
                .Select(m => new ClassPoolVM
                {
                    Id = m.Id,
                    Average = m.AverageScore,
                    StudentName = $"{m.Student.User.FirstName} {m.Student.User.LastName}",
                    Level = m.FromClass.SchoolSection.Name,
                    PreviousClass = $"{m.FromClass.Name} {m.FromClass.ClassArm}",
                    RegNumber = m.Student.RegNumber,
                    WithdrawalReason = m.WithdrawalReason,
                });

            var pagedData = await newQuery.ToPagedListAsync(vm.PageIndex, vm.PageSize);

            result.Data = new PaginatedModel<ClassPoolVM>(pagedData, vm.PageIndex, vm.PageSize, pagedData.TotalItemCount);

            return result;
        }

        public async Task<ResultModel<PaginatedModel<ClassPoolVM>>> GetRepeatList(QueryModel vm, long sessionId, long? fromClassId)
        {
            var result = new ResultModel<PaginatedModel<ClassPoolVM>>();

            var query = _promotionLogRepo.GetAll().Where(m => m.PromotionStatus == Enumeration.PromotionStatus.Repeated && m.SessionSetupId == sessionId);

            if (fromClassId != null)
            {
                query = query.Where(m => m.FromClassId == fromClassId);
            }

            var newQuery = query.OrderBy(x => x.Id)
                .Select(m => new ClassPoolVM
                {
                    Id = m.Id,
                    Average = m.AverageScore,
                    StudentName = $"{m.Student.User.FirstName} {m.Student.User.LastName}",
                    Level = m.FromClass.SchoolSection.Name,
                    PreviousClass = $"{m.FromClass.Name} {m.FromClass.ClassArm}",
                    RegNumber = m.Student.RegNumber,
                });

            var pagedData = await newQuery.ToPagedListAsync(vm.PageIndex, vm.PageSize);

            result.Data = new PaginatedModel<ClassPoolVM>(pagedData, vm.PageIndex, vm.PageSize, pagedData.TotalItemCount);

            return result;
        }

        public async Task<ResultModel<List<PromotionHighlightVM>>> GetPromotionHighlight(long sessionId, long? fromClassId)
        {
            var result = new ResultModel<List<PromotionHighlightVM>>();

            var data = await _promotionLogRepo.GetAll().Where(m => m.SessionSetupId == sessionId)
                .Select(m => new PromotionHighlightVM
                {
                    StudentId = m.StudentId,
                    Score = m.AverageScore,
                }).ToListAsync();

            result.Data = data;

            return result;
        }

    }
}
