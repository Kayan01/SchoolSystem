using LearningSvc.Core.Models;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.SchoolClass;
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
    public class SchoolClassService : ISchoolClassService
    {
        private readonly IRepository<SchoolClass, long> _schoolClassRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SchoolClassService(IUnitOfWork unitOfWork, IRepository<SchoolClass, long> schoolClassRepo)
        {
            _unitOfWork = unitOfWork;
            _schoolClassRepo = schoolClassRepo;
        }

        public async Task<ResultModel<SchoolClassVM>> AddSchoolClass(SchoolClassVM model)
        {
            var result = new ResultModel<SchoolClassVM>();

            //create class session
            var cls = new SchoolClass
            {
                //Todo : Add more fields
                Name = model.Name,
            };

            var id = _schoolClassRepo.InsertAndGetId(cls);
            await _unitOfWork.SaveChangesAsync();
            model.Id = id;
            result.Data = model;
            return result;
        }

        public async Task AddOrUpdateClassFromBroadcast(ClassSharedModel model)
        {
            var schoolClass = await _schoolClassRepo.FirstOrDefaultAsync(x => x.Id == model.Id && x.TenantId == model.TenantId);
            if (schoolClass == null)
            {
                schoolClass = _schoolClassRepo.Insert(new SchoolClass
                {
                    Id = model.Id
                });
            }

            schoolClass.TenantId = model.TenantId;
            schoolClass.Name = model.Name;
            schoolClass.ClassArm = model.ClassArm;

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task AddOrUpdateClassRangeFromBroadcast(List<ClassSharedModel> model)
        {
            //list of broadcasted class ids
            var ids = model.Select(x => x.Id).ToList();

            //get all classes from db
            var schoolClasses = await _schoolClassRepo.GetAll().Where(x => ids.Contains(x.Id)).ToListAsync();

            foreach (var cls in model)
            {
               var schClass = schoolClasses.FirstOrDefault(x => x.Id == cls.Id);
                if (schClass == null)
                {
                    schoolClasses.Add(new SchoolClass
                    {

                        Id = cls.Id,
                        ClassArm = cls.ClassArm,
                        TenantId = cls.TenantId,
                        Name = cls.Name
                    });
                }
                else
                {
                    schClass.TenantId = cls.TenantId;
                    schClass.Name = cls.Name;
                    schClass.ClassArm = cls.ClassArm;
                }
            }


            foreach (var cls in schoolClasses)
            {
              await  _schoolClassRepo.UpdateAsync(cls);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ResultModel<List<SchoolClassVM>>> GetAllSchoolClass()
        {
            var result = new ResultModel<List<SchoolClassVM>>
            {
                Data = await _schoolClassRepo.GetAll().Select(x => (SchoolClassVM)x).ToListAsync()
            };
            return result;
        }
    }
}
