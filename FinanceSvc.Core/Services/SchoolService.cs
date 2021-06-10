using FinanceSvc.Core.Interfaces;
using FinanceSvc.Core.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly IRepository<School, long> _schoolRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SchoolService(IUnitOfWork unitOfWork, IRepository<School, long> schoolRepo)
        {
            _unitOfWork = unitOfWork;
            _schoolRepo = schoolRepo;
        }

        public void AddOrUpdateSchoolFromBroadcast(SchoolSharedModel model)
        {
            var school = _schoolRepo.FirstOrDefault(x => x.Id == model.Id);
            if (school == null)
            {
                school = _schoolRepo.Insert(new School
                {
                    Id = model.Id
                });
            }

            school.Email = model.Email;
            school.PhoneNumber = model.PhoneNumber;
            school.IsActive = model.IsActive;
            school.Address = model.Address;
            school.City = model.City;
            school.DomainName = model.DomainName;
            school.Logo = model.Logo;
            school.Name = model.Name;
            school.State = model.State;
            school.WebsiteAddress = model.WebsiteAddress;

            _unitOfWork.SaveChanges();
        }

        public async Task<School> GetSchool(long id)
        {
            return await _schoolRepo.GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
