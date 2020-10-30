using LearningSvc.Core.Models;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.Assignment;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.FileStorage;
using Shared.Utils;
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
        private readonly IRepository<SchoolClass, long> _cRepo;
        private readonly IRepository<Subject, long> _sRepo;
        private readonly IRepository<Teacher, long> _tRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentService _documentService;

        public AssignmentService(IUnitOfWork unitOfWork, IRepository<Assignment, long> aRepo, 
            IRepository<AssignmentAnswer, long> aaRepo, IDocumentService documentService,
            IRepository<SchoolClass, long> cRepo, IRepository<Subject, long> sRepo, IRepository<Teacher, long> tRepo)
        {
            _unitOfWork = unitOfWork;
            _assignmentRepo = aRepo;
            _submissionRepo = aaRepo;
            _cRepo = cRepo;
            _sRepo = sRepo;
            _tRepo = tRepo;
            _documentService = documentService;
        }

        public async Task<ResultModel<string>> AddAssignment(AssignmentUploadVM assignment)
        {
            var result = new ResultModel<string>();

            var c = await _cRepo.GetAsync(assignment.ClassId);
            if (c == null)
            {
                result.AddError("Class not found");
                return result;
            }

            var s = await _sRepo.GetAsync(assignment.SubjectId);
            if (s == null)
            {
                result.AddError("Subject not found");
                return result;
            }

            var t = await _tRepo.GetAsync(assignment.TeacherId);
            if (t == null)
            {
                result.AddError("Teacher not found");
                return result;
            }

            //save logo
            var files = _documentService.TryUploadSupportingDocuments(new List<DocumentVM> { assignment.Document });

            if (files.Count() != 1)
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
                Attachment = files[0]

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

        public async Task<ResultModel<PaginatedList<AssignmentGetVM>>> GetAssignmentsForClass(long classId, int pagenumber, int pagesize)
        {
            var query = _assignmentRepo.GetAll().Where(m => m.SchoolClassId == classId)
                    .Include(m => m.AssignmentAnswers).Include(m => m.Subject).Include(m => m.SchoolClass).ThenInclude(n => n.Students)
                    .Select(x => (AssignmentGetVM)x);

            var result = new ResultModel<PaginatedList<AssignmentGetVM>>
            {
                Data = await PaginatedList<AssignmentGetVM>.CreateAsync(query, pagenumber, pagesize)
            };

            return result;
        }

        public async Task<ResultModel<PaginatedList<AssignmentGetVM>>> GetAssignmentsForTeacher(long teacherId, int pagenumber, int pagesize)
        {
            var query = _assignmentRepo.GetAll().Where(m => m.TeacherId == teacherId)
                    .Include(m => m.AssignmentAnswers).Include(m => m.Subject).Include(m => m.SchoolClass).ThenInclude(n => n.Students)
                    .Select(x => (AssignmentGetVM)x);

            var result = new ResultModel<PaginatedList<AssignmentGetVM>>
            {
                Data = await PaginatedList<AssignmentGetVM>.CreateAsync(query, pagenumber, pagesize)
            };

            return result;
        }

        public async Task<ResultModel<AssignmentSubmissionVM>> GetAssignmentSubmission(long submissionId)
        {
            var result = new ResultModel<AssignmentSubmissionVM>
            {
                Data = await _submissionRepo.GetAll().Where(m => m.Id == submissionId)
                    .Include(m => m.Assignment).Include(m => m.Student).Include(m => m.Attachment)
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
