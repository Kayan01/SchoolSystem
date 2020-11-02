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

namespace LearningSvc.Core.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IRepository<Subject, long> _subjectRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SubjectService(IUnitOfWork unitOfWork, IRepository<Subject, long> subjectRepo)
        {
            _unitOfWork = unitOfWork;
            _subjectRepo = subjectRepo;
        }

        public async Task<ResultModel<SubjectVM>> AddSubject(SubjectVM model)
        {
            var result = new ResultModel<SubjectVM>();

            //create class session
            var cls = new Subject
            {
                //Todo : Add more fields
                Name = model.Name,
            };

            var id = _subjectRepo.InsertAndGetId(cls);
            await _unitOfWork.SaveChangesAsync();
            model.Id = id;
            result.Data = model;
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
