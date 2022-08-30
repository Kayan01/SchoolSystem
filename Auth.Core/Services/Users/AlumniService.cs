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
using Auth.Core.Interfaces.Setup;

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
        private readonly IRepository<PastAlumni, long> _pastAlumniRepo;
        private readonly ISchoolPropertyService _schoolPropertyService;

        public AlumniService(
            IPublishService publishService, IStudentService studentService,
            IRepository<Alumni, long> alumniRepo,
            IRepository<Student, long> studentRepo,
            IUnitOfWork unitOfWork,
            IRepository<School, long> schoolRepo,
            IDocumentService documentService,
            UserManager<User> userManager,
            IRepository<PastAlumni, long> pastAlumniRepo,
            ISchoolPropertyService schoolPropertyService
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
            _pastAlumniRepo = pastAlumniRepo;
            _schoolPropertyService = schoolPropertyService;
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

            student.IsDeleted = true;
            student.LastModificationTime = DateTime.Now;

            await _studentRepo.UpdateAsync(student);

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

        public async Task<ResultModel<PastAlumniDetailVM>> AddPastStudents(AddPastAlumniVM model, long schoolId)
        {
            var resultModel = new ResultModel<PastAlumniDetailVM>();

            var schoolDetails = await _schoolRepo.GetAll().Where(x => x.Id == schoolId).FirstOrDefaultAsync();
            if (schoolDetails == null)
            {
                resultModel.AddError("School Not Found");
                return resultModel;
            }

            var schoolPropertyValues = await _schoolPropertyService.GetSchoolProperty();
            if (schoolPropertyValues == null)
            {
                resultModel.AddError("School Property Not Found");
                return resultModel;
            }

            if (model == null)
            {
                resultModel.AddError("Model cannot be Empty");
                return resultModel;
            }

            var data = new PastAlumni()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Sex = model.Sex,
                SchoolId = schoolId,
                Nationality = model.Nationality,
                SessionName = model.SessionName,
                State = model.State,
                StateOfOrigin = model.StateOfOrigin,
                Country = model.Country,
                Address = model.Address,
                Religion = model.Religion,
                TermName = model.TermName,
                MothersMaidenName = model.MothersMaidenName,
                AlumniReason = model.AlumniReason,
                YearOfCompletion = model.YearOfCompletion,
                DateOfBirth = model.DateOfBirth,
                CreationTime = DateTime.UtcNow,
                EmailAddress = model.EmailAddress,
                RegNumber = $"{schoolPropertyValues.Data.Prefix}{schoolPropertyValues.Data.Seperator}STT{schoolPropertyValues.Data.Seperator}{DateTime.Now.Year}{schoolPropertyValues.Data.Seperator}{model.FirstName}{model.YearOfCompletion.Month}"
            };

            try
            {
                await _pastAlumniRepo.InsertAsync(data);

                await _unitOfWork.SaveChangesAsync();

                return new ResultModel<PastAlumniDetailVM>(data: data);
            }
            catch (Exception ex)
            {
                resultModel.AddError($"Error Occured : {ex.Message}");
                return resultModel;
            }
        }

        public async Task<ResultModel<List<PastAlumniDetailVM>>> GetAllPastAlumni(QueryModel model, GetAlumniQueryVM queryVM)
        {
            var resultmodel = new ResultModel<List<PastAlumniDetailVM>>();
            var query = _pastAlumniRepo.GetAll().Where(x => x.IsDeleted == false);
            var vmList = new List<PastAlumniDetailVM>();
            if (!query.Any())
                return resultmodel;

            if (!string.IsNullOrWhiteSpace(queryVM.SessionName))
            {
                query = query.Where(x => x.SessionName == queryVM.SessionName);
            }

            resultmodel.TotalCount = query.Count();
            var data = await query.Select(x => new PastAlumniDetailVM()
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmailAddress = x.EmailAddress,
                Id = x.Id,
                RegNumber = x.RegNumber,
                Sex = x.Sex,
                DateOfBirth = x.DateOfBirth

            }).ToPagedListAsync(model.PageIndex, model.PageSize);

            vmList = data.Select(x => (PastAlumniDetailVM)x).ToList();

            resultmodel.Data = vmList;

            return resultmodel;

        }

        public async Task<ResultModel<PastAlumniDetailVM>> GetPastAlumniById(long Id)
        {
            var query = await _pastAlumniRepo.GetAll().Where(x => x.Id == Id && x.IsDeleted == false).FirstOrDefaultAsync();

            if (query == null)
            {
                return new ResultModel<PastAlumniDetailVM>($"No Alumni with Id : {Id}");
            }

            return new ResultModel<PastAlumniDetailVM>(query);
        }
    }
}