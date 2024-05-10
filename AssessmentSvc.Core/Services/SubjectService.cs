using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<SubjectService> _logger;

        public SubjectService(IUnitOfWork unitOfWork, IRepository<Subject, long> subjectRepo, ILogger<SubjectService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _subjectRepo = subjectRepo;
            _logger = logger;
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
            else
            {
                _subjectRepo.Update(subject);
            }

            subject.TenantId = model.TenantId;
            subject.Name = model.Name;
            subject.IsActive = model.IsActive;

            _unitOfWork.SaveChangesAsync();
        }


        public void RemoveSubjectFromBroadCast(SubjectSharedModel model) 
        {
            var subject = _subjectRepo.FirstOrDefault(x => x.Id == model.Id && x.TenantId == model.TenantId);
            if (subject == null)
            {
                _logger.LogInformation("Subject with provided details does not exist on this service db");
            }

            _subjectRepo.DeleteAsync(subject);
            _unitOfWork.SaveChangesAsync();

        }

    }
}
