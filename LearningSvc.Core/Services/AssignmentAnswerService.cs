using LearningSvc.Core.Interfaces;
using LearningSvc.Core.Models;
using LearningSvc.Core.ViewModels.AssignmentAnswer;
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
    public class AssignmentAnswerService : IAssignmentAnswerService
    {
        private readonly IRepository<Assignment, long> _assignmentRepo;
        private readonly IRepository<AssignmentAnswer, long> _assignmentAnswerRepo;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentService _documentService;

        public AssignmentAnswerService(IUnitOfWork unitOfWork, IRepository<Assignment, long> assignmentRepo,
            IRepository<AssignmentAnswer, long> assignmentAnswerRepo, IDocumentService documentService,
            IRepository<Student, long> studentRepo)
        {
            _unitOfWork = unitOfWork;
            _assignmentRepo = assignmentRepo;
            _assignmentAnswerRepo = assignmentAnswerRepo;
            _documentService = documentService;
            _studentRepo = studentRepo;
    }

        public async Task<ResultModel<List<AssignmentAnswerListVM>>> GetAllAnswer(long assignmentId)
        {
            var assignmentAnswer = await _assignmentAnswerRepo.GetAll()
                .Where(m => m.AssignmentId == assignmentId).Select(x => new AssignmentAnswerListVM()
                {
                    Id= x.Id,
                    ClassName = $"{x.Assignment.SchoolClassSubject.SchoolClass.Name} {x.Assignment.SchoolClassSubject.SchoolClass.ClassArm}",
                    Date = x.CreationTime,
                    StudentNumber = x.Student.UserId.ToString(),
                    StudentName = $"{x.Student.LastName} {x.Student.FirstName}",
                    Score = x.Score,
                })
                .ToListAsync();

            var result = new ResultModel<List<AssignmentAnswerListVM>>
            {
                Data = assignmentAnswer
            };

            return result;
        }

        public async Task<ResultModel<AssignmentAnswerVM>> GetAssignmentAnswer(long answerId)
        {
            var result = new ResultModel<AssignmentAnswerVM>
            {
                Data = await _assignmentAnswerRepo.GetAll().Where(m => m.Id == answerId)
                    .Select(x => new AssignmentAnswerVM
                    {
                        Id = x.Id,
                        StudentName = $"{x.Student.FirstName} {x.Student.LastName}",
                        StudentNumber = x.Student.UserId.ToString(),
                        AssignmentTitle = x.Assignment.Title,
                        Comment = x.Comment,
                        Score = x.Score,
                        Date = x.CreationTime,
                        FileId = x.FileUploadId,
                        FileType = x.Attachment.ContentType,
                        File =  _documentService.TryGetUploadedFile(x.Attachment.Path)
                    }).FirstOrDefaultAsync()
            };

            return result;
        }

        public async Task<ResultModel<string>> UpdateComment(AssignmentAnswerUpdateCommentVM model)
        {
            var result = new ResultModel<string>();

            var answer = await _assignmentAnswerRepo.FirstOrDefaultAsync(model.AssignmentAnswerId);

            if (answer == null)
            {
                result.AddError("Assignment answer not found.");

                return result;
            }
            answer.Comment += "\n\n" + model.Comment;

            await _assignmentAnswerRepo.UpdateAsync(answer);
            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<string>> UpdateScore(AssignmentAnswerUpdateScoreVM model)
        {
            var result = new ResultModel<string>();

            var answer = await _assignmentAnswerRepo.FirstOrDefaultAsync(model.AssignmentAnswerId);

            if (answer == null)
            {
                result.AddError("Answer was not found");

                return result;
            }
            answer.Score = model.Score;

            await _assignmentAnswerRepo.UpdateAsync(answer);
            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<string>> AddAssignmentAnswer(AssignmentAnswerUploadVM assignmentAnswer, long currentUserId)
        {
            var result = new ResultModel<string>();

            var assignment = await _assignmentRepo.GetAsync(assignmentAnswer.AssignmentId);
            if (assignment == null)
            {
                result.AddError("Assignment was not found");
                return result;
            }

            var student = await _studentRepo.GetAll().Where(m => m.UserId == currentUserId).FirstOrDefaultAsync();
            if (student == null)
            {
                result.AddError("Current user is not a valid Student");
                return result;
            }

            //save file
            var file = await _documentService.TryUploadSupportingDocument(assignmentAnswer.Document, Shared.Enums.DocumentType.Assignment);

            if (file == null)
            {
                result.AddError("File could not be uploaded");
                return result;
            }

            var answer = await _assignmentAnswerRepo.GetAll().Where(m => m.AssignmentId == assignmentAnswer.AssignmentId && m.StudentId == student.Id).FirstOrDefaultAsync();

            if (answer != null)
            {
                answer.Attachment = file;
            }
            else
            {
                answer = new AssignmentAnswer
                {
                    AssignmentId = assignmentAnswer.AssignmentId,
                    Attachment = file,
                    StudentId = student.Id,
                };

                _assignmentAnswerRepo.Insert(answer);
            }

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

    }
}
