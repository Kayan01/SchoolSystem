using LearningSvc.Core.Interfaces;
using LearningSvc.Core.Models;
using LearningSvc.Core.ViewModels.Attendance;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Enums;

namespace LearningSvc.Core.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepository<AttendanceSubject, long> _subjectAttendanceRepo;
        private readonly IRepository<AttendanceClass, long> _classAttendanceRepo;
        private readonly IRepository<Student, long> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceService(
            IRepository<AttendanceSubject, long> subjectAttendanceRepo,
            IRepository<AttendanceClass, long> classAttendanceRepo,
            IRepository<Student, long> studentRepository,
            IUnitOfWork unitOfWork
            )
        {
            _classAttendanceRepo = classAttendanceRepo;
            _subjectAttendanceRepo = subjectAttendanceRepo;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<string>> AddAttendanceForClass(AddClassAttendanceVM model)
        {
            var currAttendance = await _classAttendanceRepo.GetAll()
                .Where(x => x.AttendanceDate == model.Date && x.ClassId == model.ClassId)
                .ToListAsync();

            //update existing attendance
            if (currAttendance.Count > 0)
            {

                foreach (var item in model.StudentAttendanceVMs)
                {
                    var currAtt = currAttendance.FirstOrDefault(x => x.StudentId == item.StudentId);

                    if (currAtt != null)
                    {
                        currAtt.AttendanceDate = model.Date;
                        currAtt.AttendanceStatus = item.AttendanceStatus;
                        currAtt.Remark = item.Remark;
                        currAtt.ClassId = model.ClassId;

                       
                    }
                    else
                    {
                        await _classAttendanceRepo.InsertAsync(new AttendanceClass
                        {
                            AttendanceDate = model.Date,
                            AttendanceStatus = item.AttendanceStatus,
                            ClassId = model.ClassId,
                            StudentId = item.StudentId,

                            Remark = item.Remark
                        });
                    }
                }
            }
            else
            {
                foreach (var item in model.StudentAttendanceVMs)
                {
                    await _classAttendanceRepo.InsertAsync(new AttendanceClass
                    {
                        AttendanceDate = model.Date,
                        AttendanceStatus = item.AttendanceStatus,
                        ClassId = model.ClassId,
                        StudentId = item.StudentId,
                        Remark =  item.Remark
                    });

                }
            }

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<string>(data: "Attendance saved");
        }

        public async Task<ResultModel<string>> AddAttendanceForSubject(AddSubjectAttendanceVM model)
        {
            var currAttendance = await _subjectAttendanceRepo.GetAll()
                  .Where(x => x.AttendanceDate == model.Date && x.SubjectId == model.SubjectId)
                  
                  .ToListAsync();

            //update existing attendance
            if (currAttendance.Count > 0)
            {

                foreach (var item in model.StudentAttendanceVMs)
                {
                    var currAtt = currAttendance.FirstOrDefault(x => x.StudentId == item.StudentId);

                    if (currAtt != null)
                    {
                        currAtt.AttendanceDate = model.Date;
                        currAtt.AttendanceStatus = item.AttendanceStatus;
                        currAtt.SubjectId = model.SubjectId;
                        currAtt.Remark = item.Remark;
                    }
                    else
                    {
                        await _subjectAttendanceRepo.InsertAsync(new AttendanceSubject
                        {
                            AttendanceDate = model.Date,
                            AttendanceStatus = item.AttendanceStatus,
                            SubjectId = model.SubjectId,
                            StudentId = item.StudentId,
                            Remark = item.Remark
                        });
                    }
                }
            }
            else
            {
                foreach (var item in model.StudentAttendanceVMs)
                {
                    await _subjectAttendanceRepo.InsertAsync(new AttendanceSubject
                    {
                        AttendanceDate = model.Date,
                        AttendanceStatus = item.AttendanceStatus,
                        SubjectId = model.SubjectId,
                        StudentId = item.StudentId,
                        Remark = item.Remark
                    });

                }
            }

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<string>(data: "Attendance saved");
        }

        public async Task<ResultModel<List<GetStudentAttendanceSubjectVm>>> GetStudentAttendanceForSubject(GetStudentAttendanceSubjectQueryVm vm)
        {
            var query = _subjectAttendanceRepo.GetAll();


            //use student id to query if provided
            if (vm.StudentId.HasValue)
            {
                query = query.Where(x => x.StudentId == vm.StudentId);
            }

            //use student auth id to query if provided
            if (vm.StudentUserId.HasValue)
            {
                query = query.Where(x => x.Student.UserId == vm.StudentUserId);
            }


            //add subject query if provided
            if (vm.SubjectId.HasValue)
            {
                query = query.Where(x => x.SubjectId == vm.SubjectId);
            }
            //adds date query if provided
            if (vm.Date.HasValue)
            {
                query = query.Where(x => x.AttendanceDate == vm.Date.Value);
            }

            var data = await query.Select(x => new
            {
                x.SubjectId,
                SubjectName = x.SchoolClassSubject.Subject.Name,
                x.AttendanceStatus
            }).ToListAsync();

            if (!data.Any())
            {
                return new ResultModel<List<GetStudentAttendanceSubjectVm>>("No attendance for student found");
            }

            //group the data by subject
            var groupBySubjects = data.GroupBy(x => new { x.SubjectId, x.SubjectName });

            var result = new List<GetStudentAttendanceSubjectVm>();
            foreach (var subjectGroup in groupBySubjects)
            {
                result.Add(new GetStudentAttendanceSubjectVm
                {
                    NoOfTImesHeld = subjectGroup.Count(),
                    NoOfTimesAttended = subjectGroup.Count(x => x.AttendanceStatus == AttendanceState.Present),
                    SubjectName = subjectGroup.Key.SubjectName
                });
            }

            return new ResultModel<List<GetStudentAttendanceSubjectVm>>(data: result);
        }

        public async Task<ResultModel<IEnumerable<ListStudentAttendanceClassVm>>> GetStudentAttendanceForClass(GetStudentAttendanceClassQueryVm vm)
        {
            var query = _classAttendanceRepo.GetAll();

            //adds class id to query if availabale
            if (vm.ClassId.HasValue)
            {
                query = query.Where(x => x.ClassId == vm.ClassId);
            }
           
            //uses student user id to make query
            if (vm.StudentUserId.HasValue)
            {
                //get students class
                var student = await _studentRepository.GetAll().Where(x => x.UserId == vm.StudentUserId).FirstOrDefaultAsync();
                if (student == null)
                {
                    return new ResultModel<IEnumerable<ListStudentAttendanceClassVm>>("Student doesnt exist");
                }

                query = query.Where(x => x.ClassId == student.ClassId);
                query = query.Where(x => x.Student.UserId == vm.StudentUserId);

            }

            //adds student query if provided
            if (vm.StudentId.HasValue)
            {
                query = query.Where(x => x.StudentId == vm.StudentId);
            }

            //adds date query if provided
            if (vm.FromDate.HasValue && vm.ToDate.HasValue)
            {
                query = query.Where(x => x.AttendanceDate >= vm.FromDate.Value && x.AttendanceDate <= vm.ToDate);
            }



            var data = await query.Select(x => new
            {
                StudentId = x.StudentId,
                AttendanceDate = x.AttendanceDate,
                AttendanceStatus = x.AttendanceStatus,
                Reason = x.Remark,
            }).ToListAsync();

            var groupedData = data.GroupBy(x => x.StudentId).Select(x => new ListStudentAttendanceClassVm
            {
                StudentId = x.Key,
                AttendanceClassVms = x.Select(c => new GetStudentAttendanceClassVm
                {
                    AttendanceDate = c.AttendanceDate,
                    AttendanceStatus = c.AttendanceStatus,
                    Reason = c.Reason
                })

            });

            return new ResultModel<IEnumerable<ListStudentAttendanceClassVm>>
            {
                Data = groupedData
            };
        }



    }
}
