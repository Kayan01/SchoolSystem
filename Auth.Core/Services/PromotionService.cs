using Auth.Core.Interfaces;
using Auth.Core.Models;
using Auth.Core.Models.Alumni;
using Auth.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Enums;
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
        private readonly IPublishService _publishService;

        public PromotionService(
            IUnitOfWork unitOfWork,
            IRepository<Student, long> studentRepo,
            IRepository<SchoolClass, long> schoolClassRepo,
            IRepository<PromotionLog, long> promotionLogRepo,
            IRepository<Alumni, long> alumniRepo
            )
        {
            _unitOfWork = unitOfWork;
            _schoolClassRepo = schoolClassRepo;
            _studentRepo = studentRepo;
            _promotionLogRepo = promotionLogRepo;
            _alumniRepo = alumniRepo;
        }

        public async Task PromoteAllStudent(PromotionSharedModel model)
        {
            Random r = new Random();
            //get all classes in school
            var classes = await _schoolClassRepo.GetAll().Where(m=>m.TenantId == model.TenantId).OrderBy(m=>m.Sequence).AsNoTracking().ToListAsync();
            if (classes == null)
            {
                throw new Exception("Classes have not been setup");
            }
            if (classes.Any(m=> m.Sequence == 0))
            {
                throw new Exception("One or more Promotion Sequence have not been setup");
            }

            var students = await _studentRepo.GetAll().Where(m => m.TenantId == model.TenantId).ToListAsync();
            if (students == null)
            {
                throw new Exception("Students have not been setup");
            }

            var publishObj = new List<StudentSharedModel>();

            var repeatingStudentIds = model.StudentPromotionInfoList.Where(m=>!m.PassedCutoff).Select(m => m.StudentId).ToList();

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
                            _alumniRepo.Insert(new Alumni(student, model.SessionName)); //if current class is a terminal class, create an alumni record.
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
                            FromClassId = student.ClassId,
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

            _unitOfWork.SaveChanges();

            // Publish updated Students.
            await _publishService.PublishMessage(Topics.Student, BusMessageTypes.STUDENT, publishObj);

        }

    }
}
