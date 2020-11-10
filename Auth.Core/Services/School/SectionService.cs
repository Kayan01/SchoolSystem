using Auth.Core.Models;
using Auth.Core.Services.Interfaces.Class;
using Auth.Core.ViewModels.SchoolClass;
using IPagedList;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services.Class
{
    public class SectionService : ISectionService
    {
        private readonly IRepository<SchoolSection, long> _schoolSectionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SectionService(IRepository<SchoolSection, long> schoolSectionRepository, IUnitOfWork unitOfWork)
        {
            _schoolSectionRepository = schoolSectionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultModel<ClassSectionVM>> AddSection(ClassSectionVM model)
        {
            var result = new ResultModel<ClassSectionVM>();
            //todo: add more props
            var classArm = _schoolSectionRepository.Insert(new SchoolSection { Name = model.Name });

            await _unitOfWork.SaveChangesAsync();
            model.Id = classArm.Id;
            result.Data = model;
            return result;
        }

        public async Task<ResultModel<bool>> DeleteSection(long Id)
        {
            var result = new ResultModel<bool> { Data = false };

            var schoolSection = await _schoolSectionRepository.FirstOrDefaultAsync(Id);

            if (schoolSection == null)
            {
                result.AddError($"No section with id: {Id}");
                return result;
            }

            await _schoolSectionRepository.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            result.Data = true;

            return result;
        }

        public async Task<ResultModel<PaginatedModel<ClassSectionVM>>> GetAllSections(QueryModel vm)
        {
            var data = await _schoolSectionRepository.GetAll()                
                .ToPagedListAsync(vm.PageIndex, vm.PageSize);

            var result = new ResultModel<PaginatedModel<ClassSectionVM>>
            {
                Data = new PaginatedModel<ClassSectionVM>(data.Select(x => (ClassSectionVM)x), vm.PageIndex, vm.PageSize, data.TotalItemCount)
            };
            return result;
        }

        public async Task<ResultModel<ClassSectionUpdateVM>> UpdateSection(ClassSectionUpdateVM model)
        {
            var sec = await _schoolSectionRepository.FirstOrDefaultAsync(model.Id);
            var result = new ResultModel<ClassSectionUpdateVM>();

            if (sec == null)
            {
                result.AddError("Section could not be found");

                return result;
            }

            //TODO: add more props
            sec.Name = model.Name;

            await _schoolSectionRepository.UpdateAsync(sec);
            await _unitOfWork.SaveChangesAsync();
            result.Data = model;
            return result;
        }
    }
}