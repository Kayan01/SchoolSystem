using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceSvc.Core.Services
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


        public void AddOrUpdateStudentFromBroadcast(List<StudentSharedModel> models)
        {
            var idList = models.Select(m => m.Id).Distinct();
            var studentList = _studentRepo.GetAll().Where(m => idList.Contains(m.Id)).ToList();

            foreach (var model in models)
            {
                var student = studentList.FirstOrDefault(x => x.Id == model.Id && x.TenantId == model.TenantId);
                if (student == null)
                {
                    student = _studentRepo.Insert(new Student
                    {
                        Id = model.Id
                    });
                }

                student.TenantId = model.TenantId;
                student.ClassId = model.ClassId;
                student.FirstName = string.IsNullOrEmpty(model.FirstName) ? student.FirstName : model.FirstName;
                student.LastName = string.IsNullOrEmpty(model.LastName) ? student.LastName : model.LastName;
                student.Email = string.IsNullOrEmpty(model.Email) ? student.Email : model.Email;
                student.Phone = string.IsNullOrEmpty(model.Phone) ? student.Phone : model.Phone;
                student.ParentId = model.ParentId == 0 ? student.ParentId : model.ParentId;
                student.UserId = model.UserId;
                student.RegNumber = model.RegNumber;
                student.IsActive = model.IsActive;
                student.IsDeleted = model.IsDeleted;
                student.RegNumber = model.RegNumber;
                student.StudentStatusInSchool = model.StudentStatusInSchool;
            }
            _unitOfWork.SaveChanges();
        }

    }
}
