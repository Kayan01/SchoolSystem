using LearningSvc.Core.Models;
using LearningSvc.Core.Services.Interfaces;
using LearningSvc.Core.ViewModels.Assignment;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.FileStorage;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IRepository<Assignment, long> _assignmentRepo;
        private readonly IRepository<AssignmentAnswer, long> _submissionRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentService _documentService;

        public AssignmentService(IUnitOfWork unitOfWork, IRepository<Assignment, long> aRepo, 
            IRepository<AssignmentAnswer, long> aaRepo, IDocumentService documentService)
        {
            _unitOfWork = unitOfWork;
            _assignmentRepo = aRepo;
            _submissionRepo = aaRepo;
            _documentService = documentService;
        }

        public async Task<ResultModel<string>> AddAssignment(AssignmentUploadVM assignment)
        {
            var result = new ResultModel<string>();

            //save logo
            var files = _documentService.TryUploadSupportingDocuments(assignment.Documents);

            if (files.Count() != assignment.Documents.Count())
            {
                result.AddError("Some files could not be uploaded");

                return result;
            }

            var a = new Assignment
            {
                DueDate = assignment.DueDate,
                SchoolClassId = assignment.ClassId,
                SubjectId = assignment.SubjectId,
                TotalScore = assignment.TotalScore,
                TeacherId = assignment.TeacherId,
                Title = assignment.Title,
                Attachments= files

            };
            _assignmentRepo.Insert(a);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<List<AssignmentSubmissionListVM>>> GetAllSubmission(long assignmentId)
        {
            var assignment = await _assignmentRepo.GetAll().Include(m => m.SchoolClass).Include(m => m.AssignmentAnswers).ThenInclude(n => n.Student)
                    .FirstOrDefaultAsync(m => m.Id == assignmentId);

            var result = new ResultModel<List<AssignmentSubmissionListVM>>
            {
                Data = assignment.AssignmentAnswers.Select(m => (AssignmentSubmissionListVM)m).ToList()
            };

            return result;
        }

        public async Task<ResultModel<List<AssignmentGetVM>>> GetAssignmentsForTeacher(long teacherId)
        {
            var result = new ResultModel<List<AssignmentGetVM>>
            {
                Data = await _assignmentRepo.GetAll().Where(m => m.TeacherId == teacherId)
                    .Include(m => m.AssignmentAnswers).Include(m => m.Subject).Include(m => m.SchoolClass).ThenInclude(n => n.Students)
                    .Select(x => (AssignmentGetVM)x).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<AssignmentSubmissionVM>> GetAssignmentSubmission(long submissionId)
        {
            var result = new ResultModel<AssignmentSubmissionVM>
            {
                Data = await _submissionRepo.GetAll().Where(m => m.Id == submissionId)
                    .Include(m => m.Assignment).Include(m => m.Student).Include(m => m.Attachments)
                    .Select(x => (AssignmentSubmissionVM)x).FirstOrDefaultAsync()
            };
            return result;
        }

        public async Task<ResultModel<string>> UpdateComment(AssignmentSubmissionUpdateCommentVM model)
        {
            var result = new ResultModel<string>();

            var answer = await _submissionRepo.FirstOrDefaultAsync(model.AssignmentSubmissionId);

            if (answer == null)
            {
                result.AddError("Some files could not be uploaded");

                return result;
            }
            answer.Comment += "/n/n" + model.Comment;

            await _submissionRepo.UpdateAsync(answer);
            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<string>> UpdateScore(AssignmentSubmissionUpdateScoreVM model)
        {
            var result = new ResultModel<string>();

            var answer = await _submissionRepo.FirstOrDefaultAsync(model.AssignmentSubmissionId);

            if (answer == null)
            {
                result.AddError("Some files could not be uploaded");

                return result;
            }
            answer.Score = model.Score;

            await _submissionRepo.UpdateAsync(answer);
            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }
    }
}
