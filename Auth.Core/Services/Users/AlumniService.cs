using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Shared.Entities;
using Shared.PubSub;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.Enums;
using static Shared.Utils.CoreConstants;
using Auth.Core.Models.Alumni;
using Shared.Pagination;
using Shared.DataAccess.Repository;
using Shared.DataAccess.EfCore.UnitOfWork;
using Auth.Core.ViewModels.Alumni;
using Auth.Core.Models;
using IPagedList;
using Microsoft.EntityFrameworkCore;
using Shared.Reflection;
using Microsoft.OpenApi.Extensions;
using Shared.FileStorage;

namespace Auth.Core.Services
{
    public class AlumniService : IAlumniService
    {
        private readonly IPublishService _publishService;
        private readonly IStudentService _studentService;

        private readonly IRepository<Alumni,long> _alumniRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IRepository<School, long> _schoolRepo;
        private readonly IDocumentService _documentService;
        private readonly UserManager<User> _userManager;

        public AlumniService(
            IPublishService publishService, IStudentService studentService,
            IRepository<Alumni, long> alumniRepo,
            IRepository<Student, long> studentRepo,
            IUnitOfWork unitOfWork,
            IRepository<School, long> schoolRepo,
            IDocumentService documentService,
            UserManager<User> userManager
            )
        {
            _publishService = publishService;
            _studentService = studentService;
            _alumniRepo = alumniRepo;
            _unitOfWork = unitOfWork;
            _studentRepo = studentRepo;
            _schoolRepo = schoolRepo;
            _documentService = documentService;
            _userManager = userManager;

        }


    
        public async Task<ResultModel<AlumniDetailVM>> AddAlumni(AddAlumniVM vm)
        {
            //var studentResult =await _studentService.GetStudentById(vm.StudId);
            
            var student = await _studentRepo.GetAll().Include(x => x.User).Where(m => m.Id == vm.StudId).FirstOrDefaultAsync();

            if (student == null)
            {
                return new ResultModel<AlumniDetailVM>("Student not found");
                //return new ResultModel<AlumniDetailVM>(studentResult.ErrorMessages);
            }

            //var student = studentResult.Data;

            var alumni = new Alumni(student, vm.SessionName,vm.Reason);
           
            await _alumniRepo.InsertAsync(alumni);

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<AlumniDetailVM>(data: alumni);
        }

        public async Task<ResultModel<List<AlumniDetailVM>>> GetAllAlumni(QueryModel model, GetAlumniQueryVM queryVM)
        {
            var resultmodel = new ResultModel<List<AlumniDetailVM>>();
            var query = _alumniRepo.GetAll().Where(x => x.IsDeleted == false);
            var vmList = new List<AlumniDetailVM>();
            if (!query.Any())
                return resultmodel;

            if (!string.IsNullOrWhiteSpace(queryVM.SessionName))
            {
                query = query.Where(x => x.SessionName == queryVM.SessionName).Include(x => x.User);
            }

            resultmodel.TotalCount = query.Count();
            var data = await query.Select(x => new AlumniDetailVM()
            {
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                Email = x.User.Email,
                Id = x.Id,
                RegNumber = x.RegNumber,
                Sex = x.Sex,
                DateOfBirth = x.DateOfBirth

            }).ToPagedListAsync(model.PageIndex, model.PageSize);

            vmList = data.Select(x => (AlumniDetailVM)x).ToList();

            resultmodel.Data = vmList;

            return resultmodel;

        }

        public async Task<ResultModel<AlumniDetailVM>> GetAlumniById(long Id)
        {
            var query = await _alumniRepo.GetAll().Where(x => x.Id == Id && x.IsDeleted == false).FirstOrDefaultAsync();

            if (query == null)
            {
                return new ResultModel<AlumniDetailVM>($"No Alumni with Id : {Id}");
            }

            return new ResultModel<AlumniDetailVM>(query);
        }

        public async Task<ResultModel<AlumniDetailVM>> UpdateAlumni(UpdateAlumniVM model)
        {
            var alumni = await _alumniRepo.GetAll().Where(x => x.Id == model.Id && x.IsDeleted == false).FirstOrDefaultAsync();

            if (alumni == null)
            {
                return new ResultModel<AlumniDetailVM>($"No Alumni with Id : {model.Id}");
            }

           alumni = model.SetObjectProperty(alumni);

          await  _alumniRepo.UpdateAsync(alumni);

            return new ResultModel<AlumniDetailVM>(alumni);
        }

        public async Task<ResultModel<bool>> DeleteAlumni(long Id)
        {
            var alumni = await _alumniRepo.GetAll().Where(x => x.Id == Id && x.IsDeleted == false).FirstOrDefaultAsync();

            if (alumni == null)
            {
                return new ResultModel<bool>($"No Alumni with Id : {Id}");
            }

            alumni.IsDeleted = true;
            alumni.DeletionTime = DateTime.Now;

            var succeed = await _alumniRepo.UpdateAsync(alumni);

            return new ResultModel<bool>(true);
        }
    }
}