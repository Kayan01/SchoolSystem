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
using Shared.PubSub;
using static Shared.Utils.CoreConstants;

namespace LearningSvc.Core.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepository<AttendanceSubject, long> _subjectAttendanceRepo;
        private readonly IRepository<AttendanceClass, long> _classAttendanceRepo;
        private readonly IRepository<Student, long> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishService _publishService;

        public AttendanceService(
            IRepository<AttendanceSubject, long> subjectAttendanceRepo,
            IRepository<AttendanceClass, long> classAttendanceRepo,
            IRepository<Student, long> studentRepository,
            IPublishService publishService,
            IUnitOfWork unitOfWork
            )
        {
            _classAttendanceRepo = classAttendanceRepo;
            _subjectAttendanceRepo = subjectAttendanceRepo;
            _studentRepository = studentRepository;
            _publishService = publishService;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<string>> AddAttendanceForClass(AddClassAttendanceVM model)
        {
            var currAttendance = await _classAttendanceRepo.GetAll()
                .Where(x => x.AttendanceDate == model.Date && x.ClassId == model.ClassId)
                .ToListAsync();

            var absentStudentsIds = new List<long>();
            var attendanceClass = new AttendanceClass() { };
            //update existing attendance
            if (currAttendance.Count > 0)
            {
                foreach (var item in model.StudentAttendanceVMs)
                {
                    var currAtt = currAttendance.FirstOrDefault(x => x.StudentId == item.StudentId);

                    if (currAtt == null)
                    {
                        attendanceClass.AttendanceDate = model.Date;
                        attendanceClass.AttendanceStatus = item.AttendanceStatus;
                        attendanceClass.ClassId = model.ClassId;
                        attendanceClass.StudentId = item.StudentId;
                        attendanceClass.Remark = item.Remark;

                        await _classAttendanceRepo.InsertAsync(new AttendanceClass {
                            AttendanceDate = model.Date,
                            AttendanceStatus = item.AttendanceStatus,
                            ClassId = model.ClassId,
                            StudentId = item.StudentId,
                            Remark = item.Remark
                    });


                        if (item.AttendanceStatus == AttendanceState.Absent)
                        {
                            absentStudentsIds.Add(item.StudentId);
                        }

                        await _publishService.PublishMessage(Topics.ClassAttendance, BusMessageTypes.CLASSATTENDANCE, new ClassAttendanceSharedModel
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
                    attendanceClass.AttendanceDate = model.Date;
                    attendanceClass.AttendanceStatus = item.AttendanceStatus;
                    attendanceClass.ClassId = model.ClassId;
                    attendanceClass.StudentId = item.StudentId;
                    attendanceClass.Remark = item.Remark;

                    await _classAttendanceRepo.InsertAsync(new AttendanceClass {
                        AttendanceDate = model.Date,
                        AttendanceStatus = item.AttendanceStatus,
                        ClassId = model.ClassId,
                        StudentId = item.StudentId,
                        Remark = item.Remark
                });
                   
                    if (item.AttendanceStatus == AttendanceState.Absent)
                    {
                        absentStudentsIds.Add(item.StudentId);
                    }

                    await _publishService.PublishMessage(Topics.ClassAttendance, BusMessageTypes.CLASSATTENDANCE, new ClassAttendanceSharedModel
                    {
                        AttendanceDate = model.Date,
                        AttendanceStatus = item.AttendanceStatus,
                        ClassId = model.ClassId,
                        StudentId = item.StudentId,
                        Remark = item.Remark
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();

            //await _publishService.PublishMessage(Topics.ClassAttendance, BusMessageTypes.CLASSATTENDANCE, new ClassAttendanceSharedModel { 
            //    TenantId = attendanceClass.TenantId,
            //    StudentId = attendanceClass.StudentId,
            //    ClassId = attendanceClass.ClassId,
            //    AttendanceDate = attendanceClass.AttendanceDate,
            //    AttendanceStatus = attendanceClass.AttendanceStatus,
            //    Remark = attendanceClass.Remark
            //});

            await SendAttendanceEmails(absentStudentsIds);
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


        private async Task SendAttendanceEmails(List<long> studentIds)
        {
            var data = await _studentRepository.GetAll()
                .Where(x => studentIds.Contains(x.Id))
                .Select(x => new { 
                    ParentFullName = x.ParentName, 
                    x.ParentId,
                    x.ParentEmail,
                    StudentFullName = $"{x.FirstName} {x.LastName}",
                    x.RegNumber })
                .ToListAsync();

            var emaildata = new List<CreateEmailModel>();
            foreach (var item in data)
            {
                emaildata.Add(new CreateEmailModel
                {
                    User = new UserVM() { FullName = item.ParentFullName, Email = item.ParentEmail, Id = item.ParentId ?? 0},
                    EmailTemplateType = EmailTemplateType.AttendanceReport,
                    ReplacementData = new Dictionary<string, string>
                    {
                        {"ParentName", item.ParentFullName},
                        {"STUDENT_NAME", item.StudentFullName },
                        {"STUDENT_ID", item.RegNumber }
                    }
                });
            }
            
            await _publishService.PublishMessage(Topics.Notification, BusMessageTypes.NOTIFICATION, new CreateNotificationModel
            {
                Emails = emaildata                
            });
        }


        public async Task<ResultModel<List<StudentAttendanceSummaryVm>>> GetStudentAttendanceSummary(long studentId,long classId)
        {
            var query = _classAttendanceRepo.GetAll();

            //use student id to query if provided
             query = query.Where(x => x.StudentId == studentId && x.ClassId == classId).Include(x => x.Student);
                
            if (!query.Any())
            {
                return new ResultModel<List<StudentAttendanceSummaryVm>>("No attendance Summary for student found");
            }

            //Group student data by studentId
            var results = query.GroupBy(x => x.StudentId).Select(x => new StudentAttendanceSummaryVm
            {
                StudentId = x.Key,
                ClassId = classId,
                NoOfTimesPresent = query.Count(x => x.AttendanceStatus == AttendanceState.Present),
                NoOfTimesAbsent = query.Count(x => x.AttendanceStatus == AttendanceState.Absent)
            }).ToList();

            return new ResultModel<List<StudentAttendanceSummaryVm>>(data: results);
        }

    }

}
