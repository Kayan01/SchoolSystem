using Auth.Core.Interfaces.Setup;
using Auth.Core.Models.Setup;
using Auth.Core.ViewModels.Setup;
using IPagedList;
using log4net.ElasticSearch;
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

namespace Auth.Core.Services.Setup
{
    public class DepartmentService : IDepartmentService
    {
        private IRepository<Department, long> _repo;
        private IUnitOfWork _unitOfWork;
        public DepartmentService(IRepository<Department, long> repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<DepartmentVM>> AddDepartment(AddDepartmentVM model)
        {
            var result = new ResultModel<DepartmentVM>();
            //todo: add more props
            var department = _repo.Insert(new Department { Name = model.Name, IsActive = model.IsActive });
           
            await _unitOfWork.SaveChangesAsync();

            result.Data = department;
            return result;

        }

        public async Task<ResultModel<bool>> DeleteDepartment(long Id)
        {

            var result = new ResultModel<bool> { Data = false };

            await _repo.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();

            result.Data = true;

            return result;
        }

        public async Task<ResultModel<PaginatedModel<DepartmentListVM>>> GetAllDepartments(QueryModel vm)
        {
          var data = await _repo.GetAll().Select(x => (DepartmentListVM)x).ToPagedListAsync(vm.PageIndex, vm.PageSize);
            
            var pagedModel = new PaginatedModel<DepartmentListVM>(data, vm.PageIndex, vm.PageSize, data.TotalItemCount);

            var result = new ResultModel<PaginatedModel<DepartmentListVM>> { Data = pagedModel };
            return result;
        }

        public async Task<ResultModel<DepartmentVM>> GetDepartmentById(long Id)
        {
            var result = new ResultModel<DepartmentVM>();

            var classArm = await _repo.GetAll().Where(x=> x.Id == Id).FirstOrDefaultAsync();

            result.Data = classArm;

            return result;
        }

        public async Task<ResultModel<DepartmentVM>> UpdateDepartment(UpdateDepartmentVM model, long id)
        {
            var department = await _repo.FirstOrDefaultAsync(id);
            var result = new ResultModel<DepartmentVM>();

            if (department == null)
            {
                result.AddError("Department could not be found");

                return result;
            }

            //TODO: add more props
            department.Name = model.Name;
            department.IsActive = model.IsActive;

            await _repo.UpdateAsync(department);
            await _unitOfWork.SaveChangesAsync();
            result.Data = department;
            return result;
        }
    }
}
