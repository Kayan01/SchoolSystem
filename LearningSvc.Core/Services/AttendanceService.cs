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
using ClosedXML.Excel;
using System.IO;
using System.Data;
using ArrayToPdf;

namespace LearningSvc.Core.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepository<AttendanceSubject, long> _subjectAttendanceRepo;
        private readonly IRepository<AttendanceClass, long> _classAttendanceRepo;
        private readonly IRepository<Student, long> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SchoolClass, long> _schoolClassRepo;
        private readonly IPublishService _publishService;

        public AttendanceService(
            IRepository<AttendanceSubject, long> subjectAttendanceRepo,
            IRepository<AttendanceClass, long> classAttendanceRepo,
            IRepository<Student, long> studentRepository,
            IPublishService publishService,
            IUnitOfWork unitOfWork,
            IRepository<SchoolClass, long> schoolClassRepo
            )
        {
            _classAttendanceRepo = classAttendanceRepo;
            _subjectAttendanceRepo = subjectAttendanceRepo;
            _studentRepository = studentRepository;
            _publishService = publishService;
            _unitOfWork = unitOfWork;
            _schoolClassRepo = schoolClassRepo;
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

        public async Task<ResultModel<List<StudentAttendanceReportVM>>> ExportStudentAttendanceReport(AttendanceRequestVM model)
        {
            var result = new ResultModel<List<StudentAttendanceReportVM>>();

            var studentListData = new List<StudentAttendanceReportVM>();

            var GetStudentData = await _classAttendanceRepo.GetAll().Include(x => x.Student).Where(x => x.ClassId == model.ClassId).ToListAsync();
            //GetStudentData = GetStudentData.Where(x => x.ClassId == model.ClassId).ToList();

            var className = _schoolClassRepo.GetAll().Where(x => x.Id == model.ClassId).FirstOrDefault();
            if (GetStudentData == null)
            {
                result.Message = $"No attendance record for class with {model.ClassId}";
                return result;
            }

            if (model.AttendanceStartDate != null && model.AttendanceEndDate != null)
            {
                GetStudentData = GetStudentData.Where(x => x.AttendanceDate >= model.AttendanceStartDate && x.AttendanceDate <= model.AttendanceEndDate).ToList();
                if (GetStudentData == null)
                {
                    result.Message = $"No attendance record Specified with start date {model.AttendanceStartDate} and end-date of {model.AttendanceEndDate}";
                    return result;
                }
            }
            foreach (var student in GetStudentData)
            {
                var studentDate = new StudentAttendanceReportVM
                {
                    StudentId = student.StudentId,
                    FullName = student.Student.FirstName + " " +student.Student.LastName,
                    AttendanceStatus = (int)student.AttendanceStatus,
                    ClassName = className.Name
                };
                studentListData.Add(studentDate);
            }

            result.Data = studentListData;
            return result;
        }

        public async Task<ResultModel<List<StudentAttendanceReportVM>>> ExportClassAttendanceReport(AttendanceRequestVM model)
        {
            var resultModel = new ResultModel<List<StudentAttendanceReportVM>>();
            var groupedData = new List<StudentAttendanceReportVM>();
            var storeIds = new List<long>();

            if (model == null)
            {
                resultModel.AddError("Model Parameters cannot be empty");
                return resultModel;
            }
            var getClassAttendanceData = await _classAttendanceRepo.GetAll()
              .Include(x => x.SchoolClass)
              .Include(x => x.Student)
              .ToListAsync();

            if (model.ClassId != null)
            {
                getClassAttendanceData = getClassAttendanceData.Where(x => x.ClassId == model.ClassId).ToList();
            }

            if (model.tenantId != null)
            {
                getClassAttendanceData = getClassAttendanceData.Where(x => x.TenantId == model.tenantId).ToList();
            }

            if (model.AttendanceStartDate != null && model.AttendanceEndDate != null)
            {
                getClassAttendanceData = getClassAttendanceData.Where(x => x.AttendanceDate >= model.AttendanceStartDate && x.AttendanceDate <= model.AttendanceEndDate).ToList();
                if (getClassAttendanceData == null)
                {
                    resultModel.Message = $"No attendance record Specified with start date {model.AttendanceStartDate} and end-date of {model.AttendanceEndDate}";
                    return resultModel;
                }
            }

            if (getClassAttendanceData == null)
            {
                resultModel.Data = null;
                resultModel.Message = $"No attendance record found for class with Id {model.ClassId}";
                return resultModel;
            }

            foreach (var student in getClassAttendanceData)
            {
                var getData = getClassAttendanceData.Where(x => x.StudentId == student.Student.Id).ToList();
                if (model.ClassId != null)
                {
                    getData = getClassAttendanceData.Where(x => x.ClassId == model.ClassId).ToList();
                }

                if (!getClassAttendanceData.Any())
                {

                    return new ResultModel<List<StudentAttendanceReportVM>>("No attendance Summary for student found");
                }

                //Group student data by studentId
                var data = getData.GroupBy(x => x.StudentId).Select(x => new StudentAttendanceReportVM
                {
                    StudentId = x.Key,
                    FullName = student.Student.FirstName + " " + student.Student.LastName,
                    ClassName = student.SchoolClass.Name + " " + student.SchoolClass.ClassArm,
                    TotalNumberOfTimePresent = getData.Count(x => x.AttendanceStatus == AttendanceState.Present),
                    TotalNumberOfTimeAbsent = getData.Count(x => x.AttendanceStatus == AttendanceState.Absent)
                }).ToList();

                bool res = false;

                if (storeIds.Count > 0)
                {
                    res = storeIds.Contains(student.StudentId);
                }
                
                if (res == false)
                {
                    foreach (var dataItem in data)
                    {
                        groupedData.Add(dataItem);
                    }
                }                
                storeIds.Add(student.StudentId);
            }

            resultModel.Data = groupedData;
            resultModel.TotalCount = groupedData.Count;

            return resultModel;
        }

        public async Task<ResultModel<List<StudentAttendanceReportVM>>> studentSubjectAttendanceView(AttendanceRequestVM model)
        {
            var resultModel =  new ResultModel<List<StudentAttendanceReportVM>>();
            var groupedData = new List<StudentAttendanceReportVM>();
            var storeIds = new List<long>();

            var query = await _subjectAttendanceRepo.GetAll()
                .Include(x => x.Student)
                .Include(x => x.SchoolClassSubject.Subject)
                .ToListAsync();

            if (model.AttendanceStartDate != null && model.AttendanceEndDate != null)
            {
                query = query.Where(x => x.AttendanceDate.Date
                >= model.AttendanceStartDate.Value.Date &&
                x.AttendanceDate.Date <= 
                model.AttendanceEndDate.Value.Date).ToList();
                if (query == null)
                {
                    resultModel.Message = $"No attendance record Specified with start date {model.AttendanceStartDate} and end-date of {model.AttendanceEndDate}";
                    return resultModel;
                }
            }

            if (model.SubjectId != null)
            {
                query = query.Where(x => x.SubjectId == model.SubjectId).ToList();
            }

            if (query == null)
            {
                resultModel.Data = null;
                resultModel.Message = $"No attendance record found for subject with Id {model.ClassId}";
                return resultModel;
            }
            foreach (var student in query)
            {
                var getData = query.Where(x => x.StudentId == student.Student.Id).ToList();
                if (model.ClassId != null)
                {
                    getData = query.Where(x => x.SubjectId == model.SubjectId).ToList();
                }

                var data = getData.GroupBy(x => x.StudentId).Select(x => new StudentAttendanceReportVM
                {
                    StudentId = x.Key,
                    FullName = student.Student.FirstName + " " + student.Student.LastName,
                    SubjectName = student.SchoolClassSubject.Subject.Name,
                    TotalNumberOfTimePresent = getData.Count(x => x.AttendanceStatus == AttendanceState.Present),
                    TotalNumberOfTimeAbsent = getData.Count(x => x.AttendanceStatus == AttendanceState.Absent)
                }).ToList();

                bool res = false;

                if (storeIds.Count > 0)
                {
                    res = storeIds.Contains(student.StudentId);
                }

                if (res == false)
                {
                    foreach (var dataItem in data)
                    {
                        groupedData.Add(dataItem);
                    }
                }
                storeIds.Add(student.StudentId);
            }

            resultModel.Data = groupedData;
            resultModel.TotalCount = groupedData.Count;

            return resultModel;
        }

        public async Task<ResultModel<byte[]>> ExportAttendanceDataToExcel(List<StudentAttendanceReportVM> model)
        {
            var resultModel = new ResultModel<byte[]>();
            
            string isNullColumn = "";

            if (model == null)
            {
                resultModel.AddError($"Model cannot be Null");
                return resultModel;
            }
            try
            {
                using(var workbook = new XLWorkbook())
                {
                    var workSheet = workbook.Worksheets.Add("AttendanceSheet");

                    for (int i = 1; i <= 6; i++)
                    {
                        var headFormat = workSheet.Cell(1, i);
                        headFormat.Style.Font.SetBold();
                        headFormat.WorksheetRow().Height = 12;
                    }

                    var currentRow = 1;

                    workSheet.Cell(1, 1).Value = "FullName";
                    object className = workSheet.Cell(1, 2).Value = "ClassName";
                    workSheet.Cell(1, 3).Value = "StudentId";
                    workSheet.Cell(1, 4).Value = "NoOfTimePresent";
                    workSheet.Cell(1, 5).Value = "NoOfTimeAbsent";
                    object subject = workSheet.Cell(1, 6).Value = "Subject Name";

                    foreach (var data in model)
                    {
                        currentRow += 1;
                        workSheet.Cell(currentRow, 1).Value = $"{data.FullName}";
                        workSheet.Cell(currentRow, 2).Value = $"{data.ClassName}";
                        workSheet.Cell(currentRow, 3).Value = $"{data.StudentId}";
                        workSheet.Cell(currentRow, 4).Value = $"{data.TotalNumberOfTimePresent}";
                        workSheet.Cell(currentRow, 5).Value = $"{data.TotalNumberOfTimeAbsent}";
                        workSheet.Cell(currentRow, 6).Value = $"{data.SubjectName}";

                        if (data.ClassName == null)
                        {
                            isNullColumn = "ClassName";
                        }
                        else if (data.SubjectName == null)
                        {
                            isNullColumn = "SubjectName";
                        }
                    }

                    if (isNullColumn == "ClassName")
                    {
                        workSheet.Column(2).Delete();
                    }
                    else if (isNullColumn == "SubjectName")
                    {
                        workSheet.Column(6).Delete();
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        resultModel.Data = content;
                    }
                }


            }
            catch (Exception ex)
            {
                resultModel.AddError($"Exception Occured : {ex.Message}");
                return resultModel;
            }
            return resultModel;
        }

        public async Task<ResultModel<byte[]>> ExportAttendanceDataToPdf(List<StudentAttendanceReportVM> model)
        {
            var resultModel = new ResultModel<byte[]>();

            string isNullColumn = "";

            var table = new DataTable("AttendanceReport");

            table.Columns.Add("StudentId", typeof(long));
            table.Columns.Add("FullName", typeof(string));
            DataColumn className = table.Columns.Add("ClassName", typeof(string));
            table.Columns.Add("TotalNumberOfTimePresent", typeof(double));
            table.Columns.Add("TotalNumberOfTimeAbsent", typeof(long));
            DataColumn subjectName = table.Columns.Add("SubjectName", typeof(string));
            
            foreach (var item in model)
            {
                table.Rows.Add(item.StudentId, item.FullName,
                    item.ClassName, item.TotalNumberOfTimePresent,
                    item.TotalNumberOfTimeAbsent, item.SubjectName);

                if (item.ClassName == null)
                {
                    isNullColumn = "ClassName";
                }
                else if(item.SubjectName == null)
                {
                    isNullColumn = "SubjectName";
                }
            }

            if (isNullColumn == "ClassName")
            {
                table.Columns.Remove(className);
            }
            else if(isNullColumn == "SubjectName")
            {
                table.Columns.Remove(subjectName);
            }
            
            var pdf = table.ToPdf();
            resultModel.Data = pdf;

            return resultModel;
        }

    }

}
