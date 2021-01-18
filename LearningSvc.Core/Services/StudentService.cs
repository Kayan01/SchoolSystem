﻿using LearningSvc.Core.Interfaces;
using LearningSvc.Core.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace LearningSvc.Core.Services
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


        public void AddOrUpdateStudentFromBroadcast(StudentSharedModel model)
        {
            var student = _studentRepo.FirstOrDefault(x => x.Id == model.Id && x.TenantId == model.TenantId);
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
            student.IsActive = model.IsActive;
            student.IsDeleted = model.IsDeleted;

            _unitOfWork.SaveChanges();
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
    }
}
