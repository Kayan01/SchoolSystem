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

        public async Task<ResultModel<TeacherVM>> AddTeacher(TeacherVM model)
        {
            var result = new ResultModel<TeacherVM>();

            var cls = new Teacher
            {
                //Todo : Add more fields
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,

            };

            var id = _teacherRepo.InsertAndGetId(cls);
            await _unitOfWork.SaveChangesAsync();
            model.Id = id;
            result.Data = model;
            return result;
        }

        public async Task AddOrUpdateTeacherFromBroadcast(TeacherSharedModel model)
        {
            var teacher = await _teacherRepo.FirstOrDefaultAsync(x => x.Id == model.Id && x.TenantId == model.TenantId);
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

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<ResultModel<List<TeacherVM>>> GetAllTeacher()
        {
            var result = new ResultModel<List<TeacherVM>>
            {
                Data = await _teacherRepo.GetAll().Select(x => (TeacherVM)x).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<TeacherVM>> GetTeacherSummaryById(long id)
        {
            var result = new ResultModel<TeacherVM>
            {
                Data = await _teacherRepo.FirstOrDefaultAsync(id)
            };
            return result;
        }
    }
}
