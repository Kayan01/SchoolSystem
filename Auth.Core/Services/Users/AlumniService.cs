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
using IPagedList;
using Microsoft.EntityFrameworkCore;
using Shared.Reflection;

namespace Auth.Core.Services
{
    public class AlumniService : IAlumniService
    {
        private readonly IPublishService _publishService;
        private readonly IStudentService _studentService;

        private readonly IRepository<Alumni,long> _alumniRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AlumniService(
            IPublishService publishService, IStudentService studentService,
            IRepository<Alumni, long> alumniRepo,
             IUnitOfWork unitOfWork)
        {
            _publishService = publishService;
            _studentService = studentService;
            _alumniRepo = alumniRepo;
            _unitOfWork = unitOfWork;


        }


    
        public async Task<ResultModel<AlumniDetailVM>> AddAlumni(AddAlumniVM vm)
        {
            var studentResult = await _studentService.GetStudentById(vm.StudId);

            if (studentResult.HasError)
            {
                return new ResultModel<AlumniDetailVM>(studentResult.ErrorMessages);
            }

            var student = studentResult.Data;

            var alumni = new Alumni
            {
                DateOfBirth = student.DateOfBirth,
                Address = student.HomeAddress,
                AdmissionDate = student.AdmissionDate,
                Country = student.Country,
                Level = student.Level,
                LocalGovernment = student.LocalGovernment,
                MothersMaidenName = student.MothersMaidenName,
                State = student.State,
                TenantId = student.TenantId,
                StudentType = (StudentType)Enum.Parse(typeof(StudentType), student.StudentType),
                StudentId = student.Id,
                RegNumber = student.RegNumber,
                Nationality = student.Nationality,
                ParentId = student.ParentId,
                Sex = student.Sex,
                Religion = student.Religion,
                StateOfOrigin = student.StateOfOrigin,
                SessionName = vm.SessionName,
                TermName = vm.TermName,
                
            };

           await _alumniRepo.InsertAsync(alumni);

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<AlumniDetailVM>(data: alumni);
        }

        public async Task<ResultModel<PaginatedModel<AlumniDetailVM>>> GetAllAlumni(QueryModel model, GetAlumniQueryVM queryVM)
        {
            var query = _alumniRepo.GetAll();

            if (!string.IsNullOrWhiteSpace(queryVM.SessionName))
            {
                query = query.Where(x => x.SessionName == queryVM.SessionName);
            }
            if (!string.IsNullOrWhiteSpace(queryVM.TermName))
            {
                query = query.Where(x => x.TermName == queryVM.TermName);
            }

            var data = await query.ToPagedListAsync(model.PageIndex, model.PageSize);

            //convert list using reflection
            var vmList = data.Items.SetObjectPropertiesFromList(new List<AlumniDetailVM>());

            return new ResultModel<PaginatedModel<AlumniDetailVM>> { Data = new PaginatedModel<AlumniDetailVM>(vmList, data.PageNumber, data.PageSize, data.TotalItemCount) };


        }

        public async Task<ResultModel<AlumniDetailVM>> GetAlumniById(long Id)
        {
            var query = await _alumniRepo.GetAll().Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (query == null)
            {
                return new ResultModel<AlumniDetailVM>($"No Alumni with Id : {Id}");
            }

            return new ResultModel<AlumniDetailVM>(query);
        }

        public async Task<ResultModel<AlumniDetailVM>> UpdateAlumni(UpdateAlumniVM model)
        {
            var alumni = await _alumniRepo.GetAll().Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            if (alumni == null)
            {
                return new ResultModel<AlumniDetailVM>($"No Alumni with Id : {model.Id}");
            }

           alumni = model.SetObjectProperty(alumni);

          await  _alumniRepo.UpdateAsync(alumni);

            return new ResultModel<AlumniDetailVM>(alumni);
        }

        public Task<ResultModel<bool>> DeleteAlumni(long Id)
        {
            throw new NotImplementedException();
        }
    }
}