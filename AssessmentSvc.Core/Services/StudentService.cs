using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.ViewModels.Student;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork, IRepository<Student, long> studentRepo)
        {
            _unitOfWork = unitOfWork;
            _studentRepo = studentRepo;
        }


        public async Task AddOrUpdateStudentFromBroadcast(StudentSharedModel model)
        {
            var student = await _studentRepo.GetAll().Where(x => x.Id == model.Id && x.TenantId == model.TenantId).FirstOrDefaultAsync();
            if (student == null)
            {
                student = _studentRepo.Insert(new Student
                {
                    Id = model.Id
                });
            }

            student.TenantId = model.TenantId;
            student.ClassId = model.ClassId;
            student.FirstName = model.FirstName;
            student.LastName = model.LastName;
            student.Email = model.Email;
            student.Phone = model.Phone;
            student.UserId = model.UserId;
            student.RegNumber = model.RegNumber;
            student.IsActive = model.IsActive;
            student.IsDeleted = model.IsDeleted;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<long> GetStudentClassIdByUserId(long userId)
        {
            var student = await _studentRepo.GetAll().Where(m => m.UserId == userId).FirstOrDefaultAsync();
            if (student == null)
            {
                return 0;
            }
            return student.ClassId.Value;
        }

        public async Task<ResultModel<List<StudentVM>>> GetStudentsByClass(long classId)
        {
            var students = await _studentRepo.GetAll()
                .Where(m => m.ClassId == classId)
                .Select(m=>(StudentVM)m).ToListAsync();

            return new ResultModel<List<StudentVM>>()
            {
                Data = students
            };
        }
    }
}
