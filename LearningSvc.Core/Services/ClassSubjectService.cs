using LearningSvc.Core.Interfaces;
using LearningSvc.Core.Models;
using LearningSvc.Core.ViewModels.ClassSubject;
using LearningSvc.Core.ViewModels.Subject;
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
    public class ClassSubjectService: IClassSubjectService
    {
        private readonly IRepository<SchoolClassSubject, long> _classSubjectRepo;
        private readonly IRepository<Subject, long> _subjectRepo;
        private readonly IRepository<SchoolClass, long> _schoolClassRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ClassSubjectService(IUnitOfWork unitOfWork, 
            IRepository<SchoolClassSubject, long> classSubjectRepo, 
            IRepository<SchoolClass, long> schoolClassRepo, 
            IRepository<Subject, long> subjectRepo)
        {
            _unitOfWork = unitOfWork;
            _classSubjectRepo = classSubjectRepo;
            _schoolClassRepo = schoolClassRepo;
            _subjectRepo = subjectRepo;
        }

        public async Task<ResultModel<string>> AddSubjectsForClass(SubjectsToClassInsertVM model)
        {
            var r = new ResultModel<string>();
            var schoolClass = await _schoolClassRepo.GetAll().Where(x => x.Id == model.ClassId).FirstOrDefaultAsync();
            if (schoolClass == null)
            {
                r.AddError("Class was not found");
                return r;
            }

            foreach (var id in model.SubjectIds)
            {
                _classSubjectRepo.Insert(new SchoolClassSubject()
                {
                    SchoolClassId = model.ClassId,
                    SubjectId = id
                });
            }

            await _unitOfWork.SaveChangesAsync();

            r.Data = "Saved successfully";
            return r;
        }

        public async Task<ResultModel<string>> AddClassesToSubject(ClassesToSubjectInsertVM model)
        {
            var r = new ResultModel<string>();
            var subject = await _subjectRepo.GetAll().Where(x => x.Id == model.SubjectId).FirstOrDefaultAsync();
            if (subject == null)
            {
                r.AddError("Subject was not found");
                return r;
            }

            foreach (var id in model.ClassIds)
            {
                _classSubjectRepo.Insert(new SchoolClassSubject()
                {
                    SchoolClassId = id,
                    SubjectId = model.SubjectId
                });
            }

            await _unitOfWork.SaveChangesAsync();

            r.Data = "Saved successfully";
            return r;
        }

        public async Task<ResultModel<List<ClassSubjectListVM>>> GetAllClassSubjects()
        {
            var result = new ResultModel<List<ClassSubjectListVM>>
            {
                Data = await _classSubjectRepo.GetAll().Select(x => new ClassSubjectListVM() { 
                    Id = x.Id,
                    Class = $"{x.SchoolClass.Name} {x.SchoolClass.ClassArm}",
                    Subject = x.Subject.Name
                }).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<List<ClassSubjectListVM>>> GetSubjectsForClass(long classId)
        {
            var result = new ResultModel<List<ClassSubjectListVM>>
            {
                Data = await _classSubjectRepo.GetAll().Where(m=>m.SchoolClassId == classId)
                .Select(x => new ClassSubjectListVM()
                {
                    Id = x.Id,
                    Class = $"{x.SchoolClass.Name} {x.SchoolClass.ClassArm}",
                    Subject = x.Subject.Name
                }).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<List<ClassSubjectListVM>>> GetClassesForSubject(long subjectId)
        {
            var result = new ResultModel<List<ClassSubjectListVM>>
            {
                Data = await _classSubjectRepo.GetAll().Where(m => m.SubjectId == subjectId)
                .Select(x => new ClassSubjectListVM()
                {
                    Id = x.Id,
                    Class = $"{x.SchoolClass.Name} {x.SchoolClass.ClassArm}",
                    Subject = x.Subject.Name
                }).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<List<ClassSubjectWithAssignmentCountVM>>> GetSubjectsForClassWithAssignmentCount(long classId)
        {
            var result = new ResultModel<List<ClassSubjectWithAssignmentCountVM>>
            {
                Data = await _classSubjectRepo.GetAll().Where(m => m.SchoolClassId == classId)
                .Select(x => new ClassSubjectWithAssignmentCountVM()
                {
                    Id = x.Id,
                    Class = $"{x.SchoolClass.Name} {x.SchoolClass.ClassArm}",
                    Subject = x.Subject.Name,
                    AssignmentCount = x.Assignments.Count()
                }).ToListAsync()
            };
            return result;
        }
    }
}
