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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace LearningSvc.Core.Services
{
    public class SchoolClassService : ISchoolClassService
    {
        private readonly IRepository<SchoolClass, long> _schoolClassRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ZoomService _zoomService;

        public SchoolClassService(IUnitOfWork unitOfWork,
            ZoomService zoomService,
            IRepository<SchoolClass, long> schoolClassRepo)
        {
            _unitOfWork = unitOfWork;
            _zoomService = zoomService;
            _schoolClassRepo = schoolClassRepo;
        }

        public ResultModel<SchoolClassVM> AddSchoolClass(SchoolClassVM model)
        {
            var result = new ResultModel<SchoolClassVM>();

            //create class session
            var cls = new SchoolClass
            {
                //Todo : Add more fields
                Name = model.Name,
            };

            var id = _schoolClassRepo.InsertAndGetId(cls);
            _unitOfWork.SaveChanges();
            model.Id = id;
            result.Data = model;
            return result;
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
               

                if (string.IsNullOrWhiteSpace(schClass.ZoomRoomId))
                {
                    var zoomObj = _zoomService.GetZoomID($"{schClass.Name} {schClass.ClassArm}").Result;
                    if (zoomObj != null)
                    {
                        schClass.ZoomRoomId = zoomObj.id.ToString();
                        schClass.ZoomRoomStartUrl = zoomObj.start_url;
                    }
                }

                if (schClass != null)
                {
                    schClass.TenantId = cls.TenantId;
                    schClass.Name = cls.Name;
                    schClass.ClassArm = cls.ClassArm;

                    _schoolClassRepo.Update(schClass);
                }

            }

            _unitOfWork.SaveChanges();
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
