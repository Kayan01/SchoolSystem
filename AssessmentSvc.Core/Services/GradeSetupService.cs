using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.ViewModels.GradeSetup;
using log4net.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Services
{
   public class GradeSetupService : IGradeSetupService
    {
        private readonly IRepository<GradeSetup, long> _gradeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GradeSetupService> _logger;

        public GradeSetupService(IRepository<GradeSetup, long> gradeRepository, IUnitOfWork unitOfWork, ILogger<GradeSetupService> logger)
        {
            _unitOfWork = unitOfWork;
            _gradeRepository = gradeRepository;
            _logger = logger;
        }
        public async Task<ResultModel<List<GradeSetupListVM>>> AddGradeSetup(List<GradeSetupVM> models)
        {
            var result = new ResultModel<List<GradeSetupListVM>>();

            //delete old setups
            var gradeSetups = await _gradeRepository.GetAll().ToListAsync();

            if (gradeSetups.Count > 0)
            {
                foreach (var setup in gradeSetups)
                {
                    await _gradeRepository.DeleteAsync(setup);
                }
            }
            

            //add new grade setup
            var grades = models.Select(x => new GradeSetup
            {
                Grade = x.Grade,
                IsActive = x.IsActive,
                Interpretation = x.Interpretation,
                LowerBound = x.LowerBound,
                Sequence = x.Sequence,
                UpperBound = x.UpperBound
            }).ToList();

            foreach (var grade in grades)
            {
                await _gradeRepository.InsertAsync(grade);
            }

           await _unitOfWork.SaveChangesAsync();

            result.Data = grades.Select(x => new GradeSetupListVM
            {
                Grade = x.Grade,
                Id = x.Id,
                Interpretation = x.Interpretation,
                IsActive = x.IsActive,
                LowerBound = x.LowerBound,
                Sequence = x.Sequence,
                UpperBound = x.UpperBound
            }).ToList();

            return result;
        }

        public async Task<ResultModel<List<GradeSetupListVM>>> GetAllGradeForSchoolSetup()
        {
            var result = new ResultModel<List<GradeSetupListVM>> ();

            var grades = await _gradeRepository
                .GetAll()
                .OrderBy(x => x.Sequence)
                .Select(x => new GradeSetupListVM
                {
                    Grade = x.Grade,
                    Id = x.Id,
                    Interpretation = x.Interpretation,
                    IsActive = x.IsActive,
                    LowerBound = x.LowerBound,
                    Sequence = x.Sequence,
                    UpperBound = x.UpperBound
                }).ToListAsync();

            result.Data = grades;

            return result;
        }

        public async Task<ResultModel<GradeSetupVM>> GetGradeSetupById(long Id)
        {
            var result = new ResultModel<GradeSetupVM>();

            var grade = await _gradeRepository.GetAll()
                .Where(x => x.Id == Id)
                .Select(x => new GradeSetupVM
                {
                    Grade = x.Grade,
                    Interpretation = x.Interpretation,
                    IsActive = x.IsActive,
                    LowerBound = x.LowerBound,
                    Sequence = x.Sequence,
                    UpperBound = x.UpperBound
                }).FirstOrDefaultAsync();

            if (grade == null)
            {
                result.AddError($"No grade setup by id {Id}");
                return result;
            }

            result.Data = grade;

            return result;
        }

        public async Task<ResultModel<string>> UpdateGradeSetupById(GradeSetupVM model)
        {
            ResultModel<string> result = new ResultModel<string>();

            if (model.Id == null) 
            {
                
                result.AddError("ID field must be populated");
                _logger.LogInformation("Error occured because the Grade Id was not supplied");
                return result;
            }

            var query = await _gradeRepository.GetAll().Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (query != null)
            {
                result.AddError("Grade setup with provided ID does not exist");
                return result;
            }

            query.Grade = model.Grade;
            query.Interpretation = model.Interpretation;
            query.UpperBound = model.UpperBound;
            query.LowerBound = model.LowerBound;

            await _gradeRepository.UpdateAsync(query);
            await _unitOfWork.SaveChangesAsync();


            result.Data = "Grade Successfully Updated";
            result.Message = "Successful";

            return result;
        }

        public async Task<ResultModel<string>> RemoveGradeSetupById(long Id)
        {
            ResultModel<string> result = new ResultModel<string>();

            var query = await _gradeRepository.GetAll().Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (query == null)
            {
                result.AddError("No Grade setup with the supplied Id");
                return result;
            }

            query.IsDeleted = true;
            query.DeletionTime = DateTime.Now;

            result.Message = "Grade Setup successfully Removed";
            result.Data = "Successfully Removed Grade Setup";

            return result;
        }


    }
}
