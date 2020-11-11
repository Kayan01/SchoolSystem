using LearningSvc.Core.Models;
using LearningSvc.Core.Interfaces;
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
using Shared.Pagination;
using IPagedList;

namespace LearningSvc.Core.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IRepository<Subject, long> _subjectRepo;
        private readonly IRepository<SchoolClass, long> _schoolClassRepo;
        private readonly IClassSubjectService _classSubjectService;
        private readonly IUnitOfWork _unitOfWork;

        public SubjectService(IUnitOfWork unitOfWork, IRepository<Subject, long> subjectRepo, IRepository<SchoolClass, long> schoolClassRepo, IClassSubjectService classSubjectService)
        {
            _unitOfWork = unitOfWork;
            _subjectRepo = subjectRepo;
            _schoolClassRepo = schoolClassRepo;
            _classSubjectService = classSubjectService;
        }

        public async Task<ResultModel<SubjectVM>> AddSubject(SubjectInsertVM model)
        {
            var result = new ResultModel<SubjectVM>();

            var check = await _subjectRepo.GetAll().Where(m => m.Name.ToLower() == model.Name.ToLower()).FirstOrDefaultAsync();
            if (check != null)
            {
                result.AddError("Subject already Exists.");
                return result;
            }

            //create class session
            var cls = new Subject
            {
                //Todo : Add more fields
                Name = model.Name.ToLower(),
                IsActive = model.IsActive
            };

            var id = _subjectRepo.InsertAndGetId(cls);
            await _unitOfWork.SaveChangesAsync();

            if (model.ClassIds.Length > 0)
            {
                var classes = await _schoolClassRepo.GetAll().Where(x => model.ClassIds.Contains(x.Id)).Select(m=>m.Id).ToListAsync();

                if (classes.Count>0)
                {
                    await _classSubjectService.AddClassesToSubject(new ViewModels.ClassSubject.ClassesToSubjectInsertVM()
                    {
                        SubjectId = id,
                        ClassIds = classes.ToArray(),
                    });
                }
            }

            result.Data = new SubjectVM() { Id = id, Name = model.Name.ToLower(), IsActive = model.IsActive};
            return result;
        }

        public async Task<ResultModel<PaginatedModel<SubjectVM>>> GetAllSubjects(QueryModel queryModel)
        {
            var query = await _subjectRepo.GetAll().Select(x => (SubjectVM)x).ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            var result = new ResultModel<PaginatedModel<SubjectVM>>
            {
                Data = new PaginatedModel<SubjectVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
            return result;
        }

        public async Task<ResultModel<List<SubjectVM>>> GetAllSubjects()
        {
            var result = new ResultModel<List<SubjectVM>>
            {
                Data = await _subjectRepo.GetAll().Select(x => (SubjectVM)x).ToListAsync()
            };
            return result;
        }

    }
}
