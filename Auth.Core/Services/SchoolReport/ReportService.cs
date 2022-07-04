using Auth.Core.Interfaces.SchoolReport;
using Auth.Core.Models;
using Auth.Core.Models.Users;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.ReportDetail;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services.SchoolReport
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<School, long> _schoolRepo;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IRepository<Staff, long> _staffRepo;
        private readonly IRepository<SubscriptionInvoice, long> _scubcriptionRepo;
        private readonly IRepository<SchoolClass, long> _schoolClassRepo;
        private readonly IRepository<TeachingStaff, long> _teachingStaffRepo;
        private readonly IReportService _reportService;


        public ReportService(IUnitOfWork unitOfWork,
            IRepository<School,long> schoolRepo,
            IRepository<Student, long> studentRepo,
            IRepository<Staff, long> staffRepo,
            IRepository<SubscriptionInvoice, long> scubcriptionRepo,
            IRepository<SchoolClass, long> schoolClassRepo,
            IRepository<TeachingStaff, long> teachingStaffRepo)
        {
            _unitOfWork = unitOfWork;
            _schoolRepo=schoolRepo;
            _studentRepo=studentRepo;
            _staffRepo=staffRepo;
            _scubcriptionRepo=scubcriptionRepo;
            _schoolClassRepo=schoolClassRepo;
            _teachingStaffRepo=teachingStaffRepo;
        }

        /// <summary>
        /// Gets the School Details using the Schhol id to search for the details.
        /// </summary>
        /// <param name="SchoolId"></param>
        /// <returns></returns>
        public async Task<ResultModel<SchoolReportVM>> generateSchoolReport(long SchoolId)
        {
            var result = new ResultModel<SchoolReportVM>();

            var schoolData = _schoolRepo.GetAllIncluding(x => x.Students).Where(x => x.Id == SchoolId).FirstOrDefault();
            if (schoolData == null)
                return result;

            var Staff = _staffRepo.GetAll().Where(y => y.TenantId == SchoolId); ;
            var nonTeachingStaff = Staff.Where( x => x.StaffType == Shared.Enums.StaffType.NonTeachingStaff);
            var TeachingStaff = Staff.Where(x => x.StaffType == Shared.Enums.StaffType.TeachingStaff);

            var GetClassesCount = _schoolClassRepo.GetAll().Where(x => x.TenantId == SchoolId).Count();

            var data = new SchoolReportVM()
            {
                SchoolId = SchoolId,
                SchoolName = schoolData.Name,
                NumberOfStudents = schoolData.Students.Count,
                NumberOfTeachers = TeachingStaff.Count(),
                NumberOfNonAcademicStaffs = nonTeachingStaff.Count(),
                NumberOfClasses = GetClassesCount
            };

            if (data == null)
            {
                return result;
            }

            result.Data = data;

            return result;
        }

        /// <summary>
        /// Get Class Details Using ClassId 
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public async Task<ResultModel<ClassReportVM>> getClassReport(long classId)
        {
            var resultModel = new ResultModel<ClassReportVM>();

            var classData = _schoolClassRepo.GetAll().Include(x => x.Students).Where(x => x.Id == classId).FirstOrDefault();
            var classTeacher = _teachingStaffRepo.GetAll().Include(x => x.Staff.User).Where(x => x.ClassId == classData.Id).FirstOrDefault();
            
            if (classData == null || classTeacher == null)
                return resultModel;

            var data = new ClassReportVM()
            {
                ClassId = classId,
                ClassName = classData.Name + " " + classData.ClassArm,
                ClassTeacher = classTeacher.Staff.User.FullName,
                NumberOFStudentsInClass = classData.Students.Count(),
            };

            if (data == null)
                return resultModel;

            resultModel.Data = data;
            return resultModel;
        }

        /// <summary>
        /// Get All School Details For Admin
        /// </summary>
        /// <returns></returns>
        public async Task<ResultModel<IEnumerable<AdminLevelSchoolsReport>>> getSchoolDetailsForAdmin()
        {
            var resultModel = new ResultModel<IEnumerable<AdminLevelSchoolsReport>>();
            var res = new List<AdminLevelSchoolsReport>();

            //Get All Schools
            
            var getSchools = _schoolRepo.GetAll().Include(x => x.Students).Include(x => x.Staffs);
            
            //var schoolClass = await _schoolClassRepo.GetAllListAsync();
            //Get All Students In School
            
            foreach (var school in getSchools)
            {
                var students = school.Students.Count();
                var teachers = school.Staffs.Where(x => x.StaffType == Shared.Enums.StaffType.TeachingStaff).Count();
                var staffs = school.Staffs.Where(x => x.StaffType == Shared.Enums.StaffType.NonTeachingStaff).Count();
                
                //var classes = schoolClass.Where(x => x.TenantId == school.Id).Count();

                var data = new AdminLevelSchoolsReport
                {
                    SchoolName = school.Name,
                    NumberOfStudents = students,
                    NumberOfNonAcademicStaffs = staffs,
                    NumberOfAcademicStaffs = teachers,
                    //NumberOfClassInSchool = classes
                };

                res.Add(data);
            }

            resultModel.Data = res;

            return resultModel;
        }
    }
}
