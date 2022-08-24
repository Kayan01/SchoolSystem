using AssessmentSvc.Core.Context;
using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.ViewModels.Attendance;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Enums;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IRepository<AttendanceSubject, long> _subjectAttendanceRepo;
        private readonly AppDbContext _context;
        private readonly IRepository<AttendanceClass, long> _classAttendanceRepo;
        private readonly IRepository<AttendanceSubject, long> _subjectAttendance;
        private readonly IRepository<SchoolClass, long> _classRepo;

        public AttendanceService(AppDbContext context, IUnitOfWork unitOfWork,
            IRepository<AttendanceClass, long> classAttendanceRepo,
            IRepository<AttendanceSubject, long> subjectAttendanceRepo,
            IRepository<SchoolClass, long> classRepo)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _classAttendanceRepo = classAttendanceRepo;
            _subjectAttendance = subjectAttendanceRepo;
            _classRepo = classRepo;
        }
        public void AddOrUpdateAttendanceForClassFromBroadCast(ClassAttendanceSharedModel model)
        {

            var attendance = new AttendanceClass {
                StudentId = model.StudentId,
                TenantId = model.TenantId,
                AttendanceDate = model.AttendanceDate,
                AttendanceStatus = model.AttendanceStatus,
                Remark = model.Remark,
                ClassId = model.ClassId
            };

            _classAttendanceRepo.InsertAsync(attendance);

            _unitOfWork.SaveChanges();
        }

        public void AddOrUpdateAttendanceForSubjectFromBroadCast(AttendanceSubject model)
        {
            var subjectAttendance = _subjectAttendance.Insert(model);

            _unitOfWork.SaveChanges();

        }

        public async Task<ResultModel<List<StudentAttendanceSummaryVm>>> GetStudentAttendanceSummary(long? studentId, long classId)
        {
            if (studentId == null)
            {
                return new ResultModel<List<StudentAttendanceSummaryVm>>("No attendance Summary for student found");
            }

            var query = await _context.ClassAttendance.Include(x => x.Student).ToListAsync();

            //use student id to query if provided
            query = query.Where(x => x.StudentId == studentId && x.ClassId == classId).ToList();

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

            return new ResultModel<List<StudentAttendanceSummaryVm>>(results);
        }


        public async Task<ResultModel<List<StudentAttendanceReportVM>>> ExportStudentAttendanceReport(AttendanceRequestVM model)
        {
            var result = new ResultModel<List<StudentAttendanceReportVM>>();

            var GetStudentData = await _classAttendanceRepo.GetAllListAsync();
            GetStudentData = GetStudentData.Where(x => x.ClassId == model.ClassId).ToList();

            var className = _classRepo.GetAll().Where(x => x.Id == model.ClassId).FirstOrDefault();
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
                    FullName = student.Student.FirstName + student.Student.LastName,
                    AttendanceStatus = (int)student.AttendanceStatus,
                    ClassName = className.Name
                };

                result.Data.Add(studentDate);
            }
            return result;
        }
    }
}
