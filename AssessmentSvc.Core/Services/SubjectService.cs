using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Services
{
    public class SubjectService: ISubjectService
    {
        private readonly IRepository<Subject, long> _subjectRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SubjectService(IUnitOfWork unitOfWork, IRepository<Subject, long> subjectRepo)
        {
            _unitOfWork = unitOfWork;
            _subjectRepo = subjectRepo;
        }

        public void AddOrUpdateSubjectFromBroadcast(SubjectSharedModel model)
        {
            var subject = _subjectRepo.FirstOrDefault(x => x.Id == model.Id && x.TenantId == model.TenantId);
            if (subject == null)
            {
                subject = _subjectRepo.Insert(new Subject
                {
                    Id = model.Id
                });
            }

            subject.TenantId = model.TenantId;
            subject.Name = model.Name;
            subject.IsActive = model.IsActive;

            _unitOfWork.SaveChangesAsync();
        }

    }
}
