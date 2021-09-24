using Auth.Core.Interfaces;
using Auth.Core.Models;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.Subscription;
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

namespace Auth.Core.Services
{
    public class SchoolSubscriptionService : ISchoolSubscriptionService
    {
        public readonly IRepository<SchoolSubscription, long> _subscriptionRepo;
        public readonly IRepository<School, long> _schoolRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SchoolSubscriptionService(
            IRepository<SchoolSubscription, long> subscriptionRepo,
            IRepository<School, long> schoolRepo,
             IUnitOfWork unitOfWork)
        {
            _subscriptionRepo = subscriptionRepo;
            _schoolRepo = schoolRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultModel<string>> AddSchoolSubscription(List<AddSubscriptionVM> VMs)
        {
            if (VMs is null)
            {
                return new ResultModel<string>(errorMessage: "Not found");
            }

            var schoolIds = VMs.Select(m => m.SchoolId);

            var schoolList = await _schoolRepo.GetAll().Include(m=>m.SchoolSubscription).Where(m => schoolIds.Contains(m.Id)).ToListAsync();

            foreach (var school in schoolList)
            {
                var vm = VMs.First(m => m.SchoolId == school.Id);

                if (school.SchoolSubscription is null)
                {
                    school.SchoolSubscription = new SchoolSubscription()
                    {
                        PricePerStudent = vm.PricePerStudent,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        ExpectedNumberOfStudent = vm.ExpectedNumberOfStudent,
                    };
                }
                else
                {
                    school.SchoolSubscription.PricePerStudent = vm.PricePerStudent;
                    school.SchoolSubscription.StartDate = vm.StartDate;
                    school.SchoolSubscription.EndDate = vm.EndDate;
                    school.SchoolSubscription.ExpectedNumberOfStudent = vm.ExpectedNumberOfStudent;
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<string>(data: "Saved Successfully");
        }

        public async Task<ResultModel<PaginatedModel<SubscriptionVM>>> GetAllSchoolSubscription(QueryModel model, long? groupId = null)
        {
            var firstQuery = _subscriptionRepo.GetAll();

            //add where clause to filter schools
            if (groupId != null)
            {
                firstQuery = firstQuery.Where(x => x.School.SchoolGroupId == groupId);
            }
            var query = firstQuery.OrderByDescending(x => x.Id)
                 .Select(x => new SubscriptionVM() 
                 { 
                    EndDate = x.EndDate,
                    ExpectedNumberOfStudent = x.ExpectedNumberOfStudent,
                    PricePerStudent = x.PricePerStudent,
                    School = x.School.Name,
                    SchoolGroup = x.School.SchoolGroup.Name,
                    StartDate = x.StartDate,
                    IsActive = x.School.IsActive
                 });


            var pagedData = await query.ToPagedListAsync(model.PageIndex, model.PageSize);

            var data = new PaginatedModel<SubscriptionVM>(pagedData.ToList(), model.PageIndex, model.PageSize, pagedData.TotalItemCount);

            var result = new ResultModel<PaginatedModel<SubscriptionVM>>
            {
                Data = data
            };

            return result;
        }

    }
}
