using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
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
    public class TeacherService : ITeacherService
    {
        private readonly IRepository<Teacher, long> _teacherRepo;
        private readonly IUnitOfWork _unitOfWork;

        public TeacherService(IUnitOfWork unitOfWork,IRepository<Teacher, long> teacherRepo)
        {
            _unitOfWork = unitOfWork;
            _teacherRepo = teacherRepo;
        }

        public void AddOrUpdateTeacherFromBroadcast(TeacherSharedModel model)
        {
            var teacher = _teacherRepo.FirstOrDefault(x => x.Id == model.Id && x.TenantId == model.TenantId);
            if (teacher == null)
            {
                teacher = _teacherRepo.Insert(new Teacher
                {
                    Id = model.Id
                });
            }

            teacher.TenantId = model.TenantId;
            teacher.ClassId = model.ClassId;
            teacher.FirstName = model.FirstName;
            teacher.LastName = model.LastName;
            teacher.Email = model.Email;
            teacher.Phone = model.Phone;
            teacher.UserId = model.UserId;
            teacher.IsActive = model.IsActive;
            teacher.IsDeleted = model.IsDeleted;

            _unitOfWork.SaveChanges();
        }
        
    }
}
