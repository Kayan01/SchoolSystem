using ProtoBuf.Meta;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Models;
using UserManagement.Core.Services.Interfaces;
using UserManagement.Core.ViewModels;

namespace UserManagement.Core.Services
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


        public async Task<ResultModel<object>> GetAllSchools()
        {
            var result = new ResultModel<object>
            {
                Data = await _schoolRepo.GetAllListAsync()
            };
            return result;
        }

        public async Task<ResultModel<SchoolVM>> AddSchool(SchoolVM model)
        {
            var result = new ResultModel<SchoolVM>();
            var school = _schoolRepo.Insert(new School {Name= model.Name });
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

        public async Task<ResultModel<bool>> UpdateSchool(SchoolUpdateVM model)
        {
           var sch = await _schoolRepo.FirstOrDefaultAsync(model.Id);
            var result = new ResultModel<bool>();

            if (sch != null)
            {
                //TODO: add more props
                sch.Name = model.Name;



                await  _schoolRepo.UpdateAsync(sch);
                await _unitOfWork.SaveChangesAsync();
                result.Data = true;
                return result;
            }

            return result;
        }

        public async Task<ResultModel<bool>> DeleteSchool(long Id)
        {
            var result = new ResultModel<bool> { Data = false };
            await _schoolRepo.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            result.Data = true;

            return result;            
        }


    }
}
