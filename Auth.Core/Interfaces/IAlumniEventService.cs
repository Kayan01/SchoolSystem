using Auth.Core.ViewModels;
using Auth.Core.ViewModels.AlumniEvent;
using Microsoft.AspNetCore.Http;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces
{
    public interface IAlumniEventService
    {
        Task<ResultModel<AlumniEventDetailVM>> AddEvent(AddEventVM vM, IFormFile eventImage);
        Task<ResultModel<PaginatedModel<AlumniEventDetailVM>>> GetAllEvents(QueryModel model);
        Task<ResultModel<AlumniEventDetailVM>> GetEventsById(long Id);
        Task<ResultModel<AlumniEventDetailVM>> UpdateEventById(long Id, UpdateEventVM vM, IFormFile file);
    }
}
