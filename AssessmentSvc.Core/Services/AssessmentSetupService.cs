using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.ViewModels.AssessmentSetup;
using Microsoft.EntityFrameworkCore;
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
    public class AssessmentSetupService : IAssessmentSetupService
    {
        private readonly IRepository<AssessmentSetup, long> _assessmentSetupRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AssessmentSetupService(IUnitOfWork unitOfWork, 
            IRepository<AssessmentSetup, long> assessmentSetupRepo)
        {
            _unitOfWork = unitOfWork;
            _assessmentSetupRepo = assessmentSetupRepo;
        }

        public async Task<ResultModel<List<AssessmentSetupVM>>> AddAssessmentSetup(List<AssessmentSetupUploadVM> models)
        {
            var result = new ResultModel<List<AssessmentSetupVM>> ();

            if (models.Sum(m=> m.MaxScore) != 100)
            {
                result.AddError("Sum of all scores should be 100");
                return result;
            }

            result.Data = new List<AssessmentSetupVM>();

            foreach (var item in models)
            {
                var assessmentSetup = new AssessmentSetup
                {
                    SequenceNumber = item.SequenceNumber,
                    MaxScore = item.MaxScore,
                    Name = item.Name,
                    IsExam = item.IsExam
                };

                result.Data.Add(_assessmentSetupRepo.Insert(assessmentSetup));
            }
            var existingAssessments = await _assessmentSetupRepo.GetAll().ToListAsync();
            foreach (var item in existingAssessments)
            {
                _assessmentSetupRepo.Delete(item);
            }

            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<ResultModel<List<AssessmentSetupVM>>> GetAllAssessmentSetup()
        {
            var result = new ResultModel<List<AssessmentSetupVM>>
            {
                Data = await _assessmentSetupRepo.GetAll()
                    .OrderBy(m=>m.SequenceNumber)
                    .Select(x => (AssessmentSetupVM)x).AsNoTracking()
                    .ToListAsync()
            };

            return result;
        }

        public async Task<ResultModel<AssessmentSetupVM>> GetAssessmentSetup(long Id)
        {
            var result = new ResultModel<AssessmentSetupVM>
            {
                Data = await _assessmentSetupRepo.FirstOrDefaultAsync(Id)
            };
            return result;
        }

        public async Task<ResultModel<string>> UpdateAssessmentSetup(List<AssessmentSetupVM> model)
        {
            var result = new ResultModel<string>();

            var total = model.Sum(x => x.MaxScore);
            if (total < 100 || total > 100)
            {
                result.AddError("Cannot update this record because the total score is less than or more than 100");
                return result;
            }

            foreach (var item in model)
            {
                var assessment = await _assessmentSetupRepo.FirstOrDefaultAsync(item.Id);

                if (assessment == null)
                {
                    result.AddError("Record to update with provided Id not found");
                    return result;
                }

                assessment.SequenceNumber = item.SequenceNumber;
                assessment.MaxScore = item.MaxScore;
                assessment.Name = item.Name;
                assessment.IsExam = item.IsExam;

                _assessmentSetupRepo.Update(assessment);
            }

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Update Successful";
            return result;
        }


        public async Task<ResultModel<string>> RemoveAssessmentSetup(long Id)
        {
            var assessment = await _assessmentSetupRepo.FirstOrDefaultAsync(Id);

            var result = new ResultModel<string>();

            if (assessment == null)
            {
                result.AddError("Not found");
                return result;
            }
           
            _assessmentSetupRepo.Delete(assessment);
            await _unitOfWork.SaveChangesAsync();

            result.Data = "Removed Successfuly Successful";
            return result;
        }
    }
}
