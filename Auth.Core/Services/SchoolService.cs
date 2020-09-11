using ProtoBuf.Meta;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.Models;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Utils;

namespace Auth.Core.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly IRepository<School, long> _schoolRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SchoolService(IRepository<School, long> schoolRepo, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _schoolRepo = schoolRepo;
        }


        public async Task<ResultModel<List<SchoolVM>>> GetAllSchools(int pageNumber, int pageSize)
        {
            var pagedData = await PaginatedList<SchoolVM>.CreateAsync(_schoolRepo.GetAll().Select(x => new SchoolVM { Id = x.Id, Name = x.Name }), pageNumber, pageSize);

            var result = new ResultModel<List<SchoolVM>>
            {
                Data = pagedData
            };


            return result;
        }

        public async Task<ResultModel<SchoolVM>> AddSchool(SchoolVM model)
        {
            var result = new ResultModel<SchoolVM>();
            //todo: add more props
            var school = _schoolRepo.Insert(new School { Name = model.Name });
            await _unitOfWork.SaveChangesAsync();
            model.Id = school.Id;
            result.Data = model;
            return result;
        }

        public async Task<ResultModel<SchoolVM>> GetSchoolById(long Id)
        {
            var result = new ResultModel<SchoolVM>();
            var school = await _schoolRepo.FirstOrDefaultAsync(x => x.Id == Id);

            if (school == null)
            {
                return result;
            }

            result.Data = school;
            return result;
        }

        public async Task<ResultModel<SchoolUpdateVM>> UpdateSchool(SchoolUpdateVM model)
        {
            var sch = await _schoolRepo.FirstOrDefaultAsync(model.Id);
            var result = new ResultModel<SchoolUpdateVM>();


            if (sch == null)
            {
                result.AddError("School does not exist");
                return result;
            }


            //TODO: add more props
            sch.Name = model.Name;


            await _schoolRepo.UpdateAsync(sch);
            await _unitOfWork.SaveChangesAsync();
            result.Data = model;
            return result;

        }

        public async Task<ResultModel<bool>> DeleteSchool(long Id)
        {

            var result = new ResultModel<bool>();

            var sch = await _schoolRepo.FirstOrDefaultAsync(Id);
            if (sch ==null)
            {
                result.AddError("School does not exist");
                result.Data = false;
                return result;
            }
            await _schoolRepo.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            result.Data = true;

            return result;
        }


    }
}
