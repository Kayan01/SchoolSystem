using Auth.Core.Models;
using Auth.Core.Services.Interfaces.Class;
using Auth.Core.ViewModels.SchoolClass;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services.Class
{
    public class ClassArmService : IClassArmService
    {
        private readonly IRepository<ClassArm, long> _classArmRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ClassArmService(IRepository<ClassArm, long> classArmRepo, IUnitOfWork unitOfWork)
        {
            _classArmRepo = classArmRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultModel<ClassArmVM>> AddClassArm(AddClassArm model)
        {
            var result = new ResultModel<ClassArmVM>();
            //todo: add more props
            var classArm = _classArmRepo.Insert(new ClassArm { Name = model.Name, IsActive = model.Status });
            await _unitOfWork.SaveChangesAsync();
            
            result.Data = (ClassArmVM)classArm;
            return result;
        }

        public async Task<ResultModel<bool>> DeleteClassArm(long Id)
        {
            var result = new ResultModel<bool> { Data = false };

            await _classArmRepo.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();

            result.Data = true;

            return result;
        }

        public async Task<ResultModel<List<ClassArmVM>>> GetAllClassArms()
        {
            var result = new ResultModel<List<ClassArmVM>>
            {
                Data = await _classArmRepo.GetAll().Select(x => (ClassArmVM)x).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<ClassArmVM>> UpdateClassArm(UpdateClassArmVM model)
        {
            var classArm = await _classArmRepo.FirstOrDefaultAsync(model.Id);
            var result = new ResultModel<ClassArmVM>();

            if (classArm == null)
            {
                result.AddError("Class Arm could not be found");

                return result;
            }

            //TODO: add more props
            classArm.Name = model.Name;
            classArm.IsActive = model.Status;

            await _classArmRepo.UpdateAsync(classArm);
            await _unitOfWork.SaveChangesAsync();
            result.Data = (ClassArmVM)classArm;
            return result;
        }
    }
}