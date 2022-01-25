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

        public AttendanceService(AppDbContext context, IUnitOfWork unitOfWork,IRepository<AttendanceClass, long> classAttendanceRepo,IRepository<AttendanceSubject, long> subjectAttendanceRepo)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _classAttendanceRepo = classAttendanceRepo;
            _subjectAttendance = subjectAttendanceRepo;
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

        public async Task<ResultModel<List<StudentAttendanceSummaryVm>>> GetStudentAttendanceSummary(long studentId, long classId)
        {
            var query = await _context.ClassAttendance.Include(x => x.Student).ToListAsync();

            //use student id to query if provided
            //query = query.Where(x => x.StudentId == studentId && x.ClassId == classId).ToList();

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

       
    }
}
