using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Shared.PubSub;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Enums;
using Auth.Core.Models.Alumni;
using Shared.Pagination;
using Shared.DataAccess.Repository;
using Shared.DataAccess.EfCore.UnitOfWork;
using Auth.Core.ViewModels.Alumni;
using IPagedList;
using Microsoft.EntityFrameworkCore;
using Shared.Reflection;
using Auth.Core.Interfaces;
using Auth.Core.ViewModels.AlumniEvent;

namespace Auth.Core.Services
{
    public class AlumniEventService : IAlumniEventService
    {

        private readonly IRepository<AlumniEvent,long> _eventRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AlumniEventService(
            IRepository<AlumniEvent, long> eventRepo,
             IUnitOfWork unitOfWork)
        {
            _eventRepo = eventRepo;
            _unitOfWork = unitOfWork;


        }


        public async Task<ResultModel<AlumniEventDetailVM>> AddEvent(AddEventVM vM)
        {

            var alumniEvent = vM.SetObjectProperty(new AlumniEvent());

            await _eventRepo.InsertAsync(alumniEvent);

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<AlumniEventDetailVM>(data: alumniEvent);
        }

        public async Task<ResultModel<PaginatedModel<AlumniEventDetailVM>>> GetAllEvents(QueryModel model)
        {
            var query =  _eventRepo.GetAll().OrderByDescending(x=> x.StartDate);

            var data = await query.ToPagedListAsync(model.PageIndex, model.PageSize);

            //convert list using reflection
            var vmList = data.Items.SetObjectPropertiesFromList(new List<AlumniEventDetailVM>());

            return new ResultModel<PaginatedModel<AlumniEventDetailVM>> { Data = new PaginatedModel<AlumniEventDetailVM>(vmList, data.PageNumber, data.PageSize, data.TotalItemCount) };

        }

        public async Task<ResultModel<ViewModels.AlumniEvent.AlumniEventDetailVM>> GetEventsById(long Id)
        {

            var query = await _eventRepo.GetAll().Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (query == null)
            {
                return new ResultModel<AlumniEventDetailVM>($"No Event with Id : {Id}");
            }

            return new ResultModel<AlumniEventDetailVM>(query);
        }

        public async Task<ResultModel<AlumniEventDetailVM>> UpdateEventById(long Id, UpdateEventVM vM)
        {
            var alumniEvent = await _eventRepo.GetAll().Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (alumniEvent == null)
            {
                return new ResultModel<AlumniEventDetailVM>($"No Event with Id : {Id}");
            }

           alumniEvent = vM.SetObjectProperty(alumniEvent);

           await _eventRepo.UpdateAsync(alumniEvent);

           await _unitOfWork.SaveChangesAsync();

            return new ResultModel<AlumniEventDetailVM>(alumniEvent);
        }
    }
}