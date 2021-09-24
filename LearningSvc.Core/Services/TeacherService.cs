using LearningSvc.Core.Models;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.Teacher;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Services
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

        public async Task AddOrUpdateTeacherFromBroadcast(TeacherSharedModel model)
        {
            var teacher = await _teacherRepo.FirstOrDefaultAsync(x => x.Id == model.Id && x.TenantId == model.TenantId);
            if (teacher == null)
            {
                teacher = await _teacherRepo.InsertAsync(new Teacher
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
            teacher.RegNumber = model.RegNumber;

           await _unitOfWork.SaveChangesAsync();
        }

        public async Task<long> GetTeacherIdByUserId(long userId)
        {
            var teacherId = await _teacherRepo.GetAll().Where(m => m.UserId == userId).Select(n => n.Id).FirstOrDefaultAsync();
            return teacherId;
        }
    }
}
