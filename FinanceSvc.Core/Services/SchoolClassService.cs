using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceSvc.Core.Services
{
    public class SchoolClassService : ISchoolClassService
    {
        private readonly IRepository<SchoolClass, long> _schoolClassRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SchoolClassService(IUnitOfWork unitOfWork, IRepository<SchoolClass, long> schoolClassRepo)
        {
            _unitOfWork = unitOfWork;
            _schoolClassRepo = schoolClassRepo;
        }

        public void AddOrUpdateClassFromBroadcast(List<ClassSharedModel> model)
        {
            //list of broadcasted class ids
            var ids = model.Select(x => x.Id).ToList();

            //get all classes from db
            var schoolClasses = _schoolClassRepo.GetAll().Where(x => ids.Contains(x.Id)).ToList();

            foreach (var cls in model)
            {
                var schClass = schoolClasses.FirstOrDefault(x => x.Id == cls.Id);
                if (schClass == null)
                {
                    schClass = _schoolClassRepo.Insert(new SchoolClass
                    {
                        Id = cls.Id,
                    });
                }
                schClass.TenantId = cls.TenantId;
                schClass.Name = cls.Name;
                schClass.ClassArm = cls.ClassArm;
            }

            _unitOfWork.SaveChanges();
        }
    }
}
