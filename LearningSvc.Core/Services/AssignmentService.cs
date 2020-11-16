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
using Shared.Pagination;
using IPagedList;

namespace LearningSvc.Core.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IRepository<Assignment, long> _assignmentRepo;
        private readonly IRepository<AssignmentAnswer, long> _assignmentAnswerRepo;
        private readonly IRepository<SchoolClassSubject, long> _schoolClassSubjectRepo;
        private readonly IRepository<Teacher, long> _teacherRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentService _documentService;

        public AssignmentService(IUnitOfWork unitOfWork, IRepository<Assignment, long> assignmentRepo, 
            IRepository<AssignmentAnswer, long> assignmentAnswerRepo, IDocumentService documentService,
            IRepository<SchoolClassSubject, long> schoolClassSubjectRepo, IRepository<Teacher, long> teacherRepo)
        {
            _unitOfWork = unitOfWork;
            _assignmentRepo = assignmentRepo;
            _assignmentAnswerRepo = assignmentAnswerRepo;
            _schoolClassSubjectRepo = schoolClassSubjectRepo;
            _teacherRepo = teacherRepo;
            _documentService = documentService;
        }

        public async Task<ResultModel<string>> AddAssignment(AssignmentUploadVM assignment, long currentUserId)
        {
            var result = new ResultModel<string>();

            var schoolClassSubject = await _schoolClassSubjectRepo.GetAsync(assignment.ClassSubjectId);
            if (schoolClassSubject == null)
            {
                result.AddError("Class subject was not found");
                return result;
            }

            var teacher = await _teacherRepo.GetAll().Where(m=> m.UserId == currentUserId).FirstOrDefaultAsync();
            if (teacher == null)
            {
                result.AddError("Current user is not a valid Teacher");
                return result;
            }

            //save file
            var file = await _documentService.TryUploadSupportingDocument(assignment.Document, Shared.Enums.DocumentType.Assignment );

            if (file == null)
            {
                result.AddError("File could not be uploaded");

                return result;
            }

            var newAssignment = new Assignment
            {
                DueDate = assignment.DueDate,
                SchoolClassSubjectId = assignment.ClassSubjectId,
                TotalScore = assignment.TotalScore,
                TeacherId = teacher.Id,
                Title = assignment.Title,
                Attachment = file,
                OptionalComment = assignment.Comment,
            };
            _assignmentRepo.Insert(newAssignment);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<List<AssignmentSubmissionListVM>>> GetAllSubmission(long assignmentId)
        {
            var assignmentAnswer = await _assignmentAnswerRepo.GetAll()
                .Where(m => m.AssignmentId == assignmentId).Select(x=> new AssignmentSubmissionListVM()
                {
                    ClassName = $"{x.Assignment.SchoolClassSubject.SchoolClass.Name} {x.Assignment.SchoolClassSubject.SchoolClass.Name}",
                    Date = x.DateSubmitted,
                    StudentNumber = x.Student.UserId.ToString(),
                    StudentName = $"{x.Student.LastName} {x.Student.FirstName}",
                })
                .ToListAsync();

            var result = new ResultModel<List<AssignmentSubmissionListVM>>
            {
                Data = assignmentAnswer
            };

            return result;
        }

        public async Task<ResultModel<PaginatedModel<AssignmentGetVM>>> GetAssignmentsForClass(long classId, QueryModel queryModel)
        {
            var query = await _assignmentRepo.GetAll().Where(m => m.SchoolClassSubject.SchoolClassId == classId)
                    .Select(x => new AssignmentGetVM
                    {
                        Id = x.Id,
                        SubjectName = x.SchoolClassSubject.Subject.Name,
                        ClassName = $"{x.SchoolClassSubject.SchoolClass.Name} {x.SchoolClassSubject.SchoolClass.Name}",
                        CreationDate = x.CreationTime,
                        DueDate = x.DueDate,
                        NumberOfStudentsSubmitted = x.AssignmentAnswers.Count(),
                        TotalStudentsInClass = x.SchoolClassSubject.SchoolClass.Students.Count(),
                        FileId = x.FileUploadId.Value,
                        ClassSubjectId = x.SchoolClassSubjectId,
                        Name = x.Title,
                        OptionalComment = x.OptionalComment,
                        TeacherName = $"{x.Teacher.FirstName} {x.Teacher.LastName}",
                    })
                    .ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            var result = new ResultModel<PaginatedModel<AssignmentGetVM>>
            {
                Data = new PaginatedModel<AssignmentGetVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
            return result;
        }

        public async Task<ResultModel<PaginatedModel<AssignmentGetVM>>> GetAssignmentsForTeacher(long currentUserId, QueryModel queryModel)
        {
            var teacher = await _teacherRepo.GetAll().Where(m => m.UserId == currentUserId).FirstOrDefaultAsync();
            if (teacher == null)
            {
                var r = new ResultModel<PaginatedModel<AssignmentGetVM>>();
                r.AddError("Current user is not a valid Teacher");
                return r;
            }

            var query = await _assignmentRepo.GetAll().Where(m => m.TeacherId == teacher.Id)
                    .Select(x => new AssignmentGetVM
                    {
                        Id = x.Id,
                        SubjectName = x.SchoolClassSubject.Subject.Name,
                        ClassName = $"{x.SchoolClassSubject.SchoolClass.Name} {x.SchoolClassSubject.SchoolClass.Name}",
                        CreationDate = x.CreationTime,
                        DueDate = x.DueDate,
                        NumberOfStudentsSubmitted = x.AssignmentAnswers.Count(),
                        TotalStudentsInClass = x.SchoolClassSubject.SchoolClass.Students.Count(),
                        FileId = x.FileUploadId.Value,
                        ClassSubjectId = x.SchoolClassSubjectId,
                        Name = x.Title,
                        OptionalComment = x.OptionalComment,
                        TeacherName = $"{x.Teacher.FirstName} {x.Teacher.LastName}",
                    })
                    .ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            var result = new ResultModel<PaginatedModel<AssignmentGetVM>>
            {
                Data = new PaginatedModel<AssignmentGetVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
            return result;
        }

        public async Task<ResultModel<AssignmentSubmissionVM>> GetAssignmentSubmission(long submissionId)
        {
            var result = new ResultModel<AssignmentSubmissionVM>
            {
                Data = await _assignmentAnswerRepo.GetAll().Where(m => m.Id == submissionId)
                    .Select(x => new AssignmentSubmissionVM
                    {
                        Id = x.Id,
                        StudentName = $"{x.Student.FirstName} {x.Student.LastName}",
                        StudentNumber = x.Student.UserId.ToString(),
                        AssignmentTitle = x.Assignment.Title,
                        Comment = x.Comment,
                        Score = x.Score,
                        Date = x.DateSubmitted,
                        FileId = x.FileUploadId
                    }).FirstOrDefaultAsync()
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
