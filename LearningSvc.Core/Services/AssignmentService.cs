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
using Microsoft.AspNetCore.Mvc;

namespace LearningSvc.Core.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IRepository<Assignment, long> _assignmentRepo;
        private readonly IRepository<AssignmentAnswer, long> _assignmentAnswerRepo;
        private readonly IRepository<SchoolClass, long> _schoolClassRepo;
        private readonly IRepository<Subject, long> _subjectRepo;
        private readonly IRepository<Teacher, long> _teacherRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentService _documentService;

        public AssignmentService(IUnitOfWork unitOfWork, IRepository<Assignment, long> assignmentRepo, 
            IRepository<AssignmentAnswer, long> assignmentAnswerRepo, IDocumentService documentService,
            IRepository<SchoolClass, long> schoolClassRepo, IRepository<Subject, long> subjectRepo, IRepository<Teacher, long> teacherRepo)
        {
            _unitOfWork = unitOfWork;
            _assignmentRepo = assignmentRepo;
            _assignmentAnswerRepo = assignmentAnswerRepo;
            _schoolClassRepo = schoolClassRepo;
            _subjectRepo = subjectRepo;
            _teacherRepo = teacherRepo;
            _documentService = documentService;
        }

        public async Task<ResultModel<string>> AddAssignment(AssignmentUploadVM assignment)
        {
            var result = new ResultModel<string>();

            var schoolClass = await _schoolClassRepo.GetAsync(assignment.ClassId);
            if (schoolClass == null)
            {
                result.AddError("Class not found");
                return result;
            }

            var subject = await _subjectRepo.GetAsync(assignment.SubjectId);
            if (subject == null)
            {
                result.AddError("Subject not found");
                return result;
            }

            var teacher = await _teacherRepo.GetAsync(assignment.TeacherId);
            if (teacher == null)
            {
                result.AddError("Teacher not found");
                return result;
            }

            //save file
            var file = await _documentService.TryUploadSupportingDocument(assignment.Document, Shared.Enums.DocumentType.Assignment );

                if (file != null)
                {
                    result.AddError("File could not be uploaded");

                    return result;
                }

            var newAssignment = new Assignment
            {
                DueDate = assignment.DueDate,
                SchoolClassId = assignment.ClassId,
                SubjectId = assignment.SubjectId,
                TotalScore = assignment.TotalScore,
                TeacherId = assignment.TeacherId,
                Title = assignment.Title,
                Attachment = file

            };
            _assignmentRepo.Insert(newAssignment);

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
                Data = await _assignmentAnswerRepo.GetAll().Where(m => m.Id == submissionId)
                    .Include(m => m.Assignment).Include(m => m.Student).Include(m => m.Attachment)
                    .Select(x => (AssignmentSubmissionVM)x).FirstOrDefaultAsync()
            };
            return result;
        }

        public async Task<ResultModel<string>> UpdateComment(AssignmentSubmissionUpdateCommentVM model)
        {
            var result = new ResultModel<string>();

            var answer = await _assignmentAnswerRepo.FirstOrDefaultAsync(model.AssignmentSubmissionId);

            if (answer == null)
            {
                result.AddError("Some files could not be uploaded");

                return result;
            }
            answer.Comment += "/n/n" + model.Comment;

            await _assignmentAnswerRepo.UpdateAsync(answer);
            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<string>> UpdateScore(AssignmentSubmissionUpdateScoreVM model)
        {
            var result = new ResultModel<string>();

            var answer = await _assignmentAnswerRepo.FirstOrDefaultAsync(model.AssignmentSubmissionId);

            if (answer == null)
            {
                result.AddError("Some files could not be uploaded");

                return result;
            }
            answer.Score = model.Score;

            await _assignmentAnswerRepo.UpdateAsync(answer);
            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }
    }
}
