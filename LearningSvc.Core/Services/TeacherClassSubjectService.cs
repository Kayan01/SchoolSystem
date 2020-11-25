using LearningSvc.Core.Interfaces;
using LearningSvc.Core.Models;
using LearningSvc.Core.ViewModels.TeacherClassSubject;
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
    public class TeacherClassSubjectService : ITeacherClassSubjectService
    {
        private readonly IRepository<TeacherClassSubject, long> _teacherClassSubjectRepo;
        private readonly IRepository<Teacher, long> _teacherRepo;
        private readonly IUnitOfWork _unitOfWork;

        public TeacherClassSubjectService(IUnitOfWork unitOfWork, IRepository<TeacherClassSubject, long> teacherClassSubjectRepo, IRepository<Teacher, long> teacherRepo)
        {
            _unitOfWork = unitOfWork;
            _teacherClassSubjectRepo = teacherClassSubjectRepo;
            _teacherRepo = teacherRepo;
        }

        public async Task<ResultModel<string>> AddTeacherToClassSubjects(TeacherClassSubjectInsertVM model)
        {
            var r = new ResultModel<string>();
            var teacher = await _teacherRepo.FirstOrDefaultAsync(x => x.Id == model.TeacherId);
            if (teacher == null)
            {
                r.AddError("Teacher was not found");
                return r;
            }

            foreach (var id in model.ClassSubjectIds)
            {
                _teacherClassSubjectRepo.Insert(new TeacherClassSubject()
                {
                    TeacherId = model.TeacherId,
                    SchoolClassSubjectId = id
                });
            }

            await _unitOfWork.SaveChangesAsync();

            r.Data = "Saved successfully";
            return r;
        }

        public async Task<ResultModel<List<TeacherClassSubjectListVM>>> GetAllTeacherClassSubjects(long teacherId)
        {
            var result = new ResultModel<List<TeacherClassSubjectListVM>>
            {
                Data = await _teacherClassSubjectRepo.GetAll().Where(x=>x.TeacherId == teacherId)
                .Select(x => new TeacherClassSubjectListVM()
                {
                    Id = x.Id,
                    Class = x.SchoolClassSubject.SchoolClass.Name,
                    Subject = x.SchoolClassSubject.Subject.Name,
                    Teacher = x.Teacher.FirstName + " " + x.Teacher.LastName
                }).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<List<TeacherClassSubjectListVM>>> GetTeachersForClassSubject(long classSubjectId)
        {
            var result = new ResultModel<List<TeacherClassSubjectListVM>>
            {
                Data = await _teacherClassSubjectRepo.GetAll().Where(x => x.SchoolClassSubjectId == classSubjectId)
                .Select(x => new TeacherClassSubjectListVM()
                {
                    Id = x.Id,
                    Class = x.SchoolClassSubject.SchoolClass.Name,
                    Subject = x.SchoolClassSubject.Subject.Name,
                    Teacher = x.Teacher.FirstName + " " + x.Teacher.LastName
                }).ToListAsync()
            };
            return result;
        }
    }
}
