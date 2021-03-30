using Auth.Core.Interfaces.Setup;
using Auth.Core.Models.Setup;
using Auth.Core.ViewModels.Setup;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services.Setup
{

    public class SchoolPropertyService : ISchoolPropertyService
    {
        private readonly IRepository<SchoolProperty, long> _schoolPropRepo;
        private readonly IUnitOfWork _unitOfWork;
        public SchoolPropertyService(
            IRepository<SchoolProperty, long> schoolPropRepo,
             IUnitOfWork unitOfWork)
        {
            _schoolPropRepo = schoolPropRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<SchoolPropertyVM>> GetSchoolProperty()
        {
            var result = new ResultModel<SchoolPropertyVM>();

            var schProp = await _schoolPropRepo.GetAll().FirstOrDefaultAsync();

            if (schProp == null)
            {
                result.AddError("School property settings doesnt exist");

                return result;
            }

            result.Data = new SchoolPropertyVM
            {
                ClassDays = schProp.ClassDays,
                EnrollmentAmount = schProp.EnrollmentAmount,
                NumberOfTerms = schProp.NumberOfTerms,
                Prefix = schProp.Prefix,
                Seperator = schProp.Seperator
            };

            return result;
        }

        public async Task<ResultModel<SchoolPropertyVM>> SetSchoolProperty(SchoolPropertyVM model)
        {
            var result = new ResultModel<SchoolPropertyVM>();


            //check if prefix and seperator has been setup for any other school
            var check = await _schoolPropRepo.GetAll()
                .Where(x => !x.IsDeleted && 
                x.Prefix == model.Prefix && 
                x.Seperator == model.Seperator)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync();

            if (check != null)
            {
                return new ResultModel<SchoolPropertyVM>("A different school has been setup with same prefix and seperator");
            }

            //check if setting has been setup before
            var schprop =  await _schoolPropRepo.GetAll().FirstOrDefaultAsync();

            if (schprop == null)
            {

                var schProp = new SchoolProperty
                {
                    Seperator = model.Seperator,
                    Prefix = model.Prefix,
                    NumberOfTerms = model.NumberOfTerms,
                    EnrollmentAmount = model.EnrollmentAmount,
                    ClassDays = model.ClassDays
                };
               await _schoolPropRepo.InsertAsync(schProp);
            }
            else
            {
                schprop.ClassDays = model.ClassDays;
                schprop.EnrollmentAmount = model.EnrollmentAmount;
                schprop.NumberOfTerms = model.NumberOfTerms;
                schprop.Prefix = model.Prefix;
                schprop.Seperator = model.Seperator;                
            }

            await _unitOfWork.SaveChangesAsync();

            result.Data = model;
            return result;

        }
    }
}
