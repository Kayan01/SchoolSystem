using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Models;
using UserManagement.Core.Services.Interfaces;
using UserManagement.Core.ViewModels.Student;

namespace UserManagement.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IRepository<School, long> _schoolRepo;
        private readonly IUnitOfWork _unitOfWork;
        public StudentService(IRepository<Student, long> studentRepo, IRepository<School, long> schoolRepo, IUnitOfWork unitOfWork)
        {
            _studentRepo = studentRepo;
            _schoolRepo = schoolRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<object>> GetAllStudentsInSchool(long schoolId)
        {
            var result = new ResultModel<object>
            {
                Data = await _studentRepo.GetAll().Where(x => x.SchoolId == schoolId)
                .Select(x => new StudentVM { Id = x.Id, FirstName = x.FirstName, LastName = x.LastName })
                .ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<StudentVM>> GetStudentById(long Id, long schoolId)
        {
            var result = new ResultModel<StudentVM>();
            var std = await _studentRepo.FirstOrDefaultAsync(x => x.Id == Id && x.SchoolId == schoolId);

            if (std == null)
            {
                result.AddError("Student does not exist");
                return result;
            }

            result.Data = std;
            return result;
        }
        public async Task<ResultModel<StudentVM>> AddStudentToSchool(long schoolId, StudentVM model)
        {
            var result = new ResultModel<StudentVM>();
            //check if school exists
            var sch = _schoolRepo.FirstOrDefault(schoolId);

            if (sch == null)
            {
                result.AddError("School does not exist");
                return result;
            }

            var staff = _studentRepo.Insert(new Student { SchoolId = schoolId, FirstName = model.FirstName, LastName = model.LastName });
            await _unitOfWork.SaveChangesAsync();
            model.Id = staff.Id;
            result.Data = model;
            return result;
        }

        public async Task<ResultModel<bool>> DeleteStudent(long Id)
        {
            var result = new ResultModel<bool> { Data = false };

            //check if the student exists
            var std = await _studentRepo.FirstOrDefaultAsync(Id);
            if (std == null)
            {
                result.AddError("Student does not exist");
                return result;
            }

            await _studentRepo.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            result.Data = true;

            return result;
        }


        public async Task<ResultModel<StudentUpdateVM>> UpdateStudent(StudentUpdateVM model)
        {
            var std = await _studentRepo.FirstOrDefaultAsync(model.Id);
            var result = new ResultModel<StudentUpdateVM>();

            if (std == null)
            {
                result.AddError("Student does not exist");
                return result;
            }

            //TODO: add more props
            std.FirstName = model.FirstName;
            await _studentRepo.UpdateAsync(std);
            await _unitOfWork.SaveChangesAsync();
            result.Data = model;
            return result;

        }
    }
}
