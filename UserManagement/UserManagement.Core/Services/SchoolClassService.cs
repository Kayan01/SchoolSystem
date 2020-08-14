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
using UserManagement.Core.ViewModels.SchoolClass;

namespace UserManagement.Core.Services
{
    public class SchoolClassService : ISchoolClassService
    {
        private readonly IRepository<SchoolClass, long> _classRepo;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IUnitOfWork _unitOfWork;
        public SchoolClassService(IRepository<SchoolClass, long> classRepo, IRepository<Student, long> studentRepo)
        {
            _classRepo = classRepo;
            _studentRepo = studentRepo;
        }
        public async Task<ResultModel<ClassVM>> AddClass(ClassVM model)
        {
            var result = new ResultModel<ClassVM>();
            //todo: add more props
            var @class = _classRepo.Insert(new SchoolClass { Name = model.Name });
            await _unitOfWork.SaveChangesAsync();
            model.Id = @class.Id;
            result.Data = model;
            return result;
        }

        public async Task<ResultModel<object>> AddStudentToClass(long classId, long studId)
        {
            var result = new ResultModel<object>();

            //get student
            var stud = await _studentRepo.FirstOrDefaultAsync(studId);

            if (stud== null)
            {
                result.AddError("Student not found");
                return result;
            }

            //get class
            var @class = await _classRepo.FirstOrDefaultAsync(classId);

            if (@class == null)
            {
                result.AddError("Class not found");
                return result;
            }


            //assign student to class
            @class.Students.Add(stud);

            await _unitOfWork.SaveChangesAsync();

            result.Message = "Student added successfully";
            return result;
        }

        public Task<ResultModel<bool>> AssignSubjectToClass(long classId, long subjId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<bool>> AssignTeacherToClass(long classId, long staffId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultModel<bool>> DeleteClass(long Id)
        {
            var result = new ResultModel<bool> { Data = false };
            await _classRepo.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            result.Data = true;

            return result;
        }

        public async Task<ResultModel<object>> GetAllClasses(long schId)
        {
            var result = new ResultModel<object>
            {
                Data = await _classRepo.GetAll().Where(x => x.SchoolId == schId).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<object>> GetClassByIdWithStudents(long Id)
        {
            var result = new ResultModel<object>();

            //gets class with students. Casting of students to studentsVM is done in the classVM
            var @class = await _classRepo.GetAll().Include(x => x.Students).Select(x => (ClassVM)x).FirstOrDefaultAsync(x => x.Id == Id);

            if (@class == null)
            {
                result.AddError("Class does not exist");
                return result;
            }

            result.Data = @class;
            return result;
        }

        public async Task<ResultModel<ClassVM>> GetClassById(long Id)
        {
            var result = new ResultModel<ClassVM>();
            var @class = await _classRepo.GetAll().Include(x => x.Students).Where(x => x.Id == Id).Select(x => new ClassVM { Id = x.Id, Name = x.Name }).FirstOrDefaultAsync();

            if (@class == null)
            {
                result.AddError("Class does not exist");
                return result;
            }

            result.Data = @class;
            return result;
        }

        public async Task<ResultModel<ClassUpdateVM>> UpdateSchool(ClassUpdateVM model)
        {
            var @class = await _classRepo.FirstOrDefaultAsync(model.Id);
            var result = new ResultModel<ClassUpdateVM>();

            if (@class != null)
            {
                //TODO: add more props
                @class.Name = model.Name;



                await _classRepo.UpdateAsync(@class);
                await _unitOfWork.SaveChangesAsync();
                result.Data = model;
                return result;
            }

            return result;
        }
    }
}
