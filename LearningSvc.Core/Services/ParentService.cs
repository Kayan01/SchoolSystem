using LearningSvc.Core.Models;
using LearningSvc.Core.Services.Interfaces;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.Services
{
    public class ParentService : IParentService
    {

        private readonly IRepository<Parent, long> _parentRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ParentService(IUnitOfWork unitOfWork, IRepository<Parent, long> parentRepo)
        {
            _unitOfWork = unitOfWork;
            _parentRepo = parentRepo;
        }


        public void AddOrUpdateParentFromBroadcast(ParentSharedModel model)
        {
            var parent = _parentRepo.FirstOrDefault(x => x.Id == model.Id);
            if (parent == null)
            {
                parent = _parentRepo.Insert(new Parent
                {
                    Id = model.Id
                });
            }

            parent.OfficeAddress = model.OfficeAddress;
            parent.LastName = model.LastName;
            parent.FirstName = model.FirstName;
            parent.Email = model.Email;
            parent.Phone = model.Phone;
            parent.UserId = model.UserId;
            parent.IsActive = model.IsActive;
            parent.IsDeleted = model.IsDeleted;
            parent.SecondaryEmail = model.SecondaryEmail;
            parent.SecondaryPhoneNumber = model.SecondaryPhoneNumber;
            parent.HomeAddress = model.HomeAddress;
            parent.RegNumber = model.RegNumber;

            _unitOfWork.SaveChanges();
        }

    }
}
