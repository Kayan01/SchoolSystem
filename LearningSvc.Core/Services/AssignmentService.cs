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
using System.IO;

namespace LearningSvc.Core.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IRepository<Assignment, long> _assignmentRepo;
        private readonly IRepository<SchoolClassSubject, long> _schoolClassSubjectRepo;
        private readonly IRepository<Teacher, long> _teacherRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentService _documentService;

        public AssignmentService(IUnitOfWork unitOfWork, IRepository<Assignment, long> assignmentRepo,  IDocumentService documentService,
            IRepository<SchoolClassSubject, long> schoolClassSubjectRepo, IRepository<Teacher, long> teacherRepo)
        {
            _unitOfWork = unitOfWork;
            _assignmentRepo = assignmentRepo;
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

        public async Task<ResultModel<AssignmentVM>> AssignmentDetail(long id)
        {
            var result = new ResultModel<AssignmentVM>();

            var query = await _assignmentRepo.GetAll().Where(m => m.Id == id)
                .Select(x => new AssignmentVM
                {
                    Id = x.Id,
                    ClassName = $"{x.SchoolClassSubject.SchoolClass.Name} {x.SchoolClassSubject.SchoolClass.ClassArm}",
                    CreationDate = x.CreationTime,
                    SubjectName = x.SchoolClassSubject.Subject.Name,
                    TeacherName = $"{x.Teacher.FirstName} {x.Teacher.LastName}",
                    FileType = x.Attachment.ContentType,
                    FileName = x.Attachment.Path,
                }).FirstOrDefaultAsync();

            if (query != null)
            {
                var filepath = Path.Combine("Filestore", query.FileName);

                if (File.Exists(filepath))
                {
                    query.File = File.ReadAllBytes(filepath);
                    query.FileSize = $"{(query.File.Length / 1000).ToString("0.00")}KB";
                }
            }

            result.Data = query;
            return result;
        }

        public async Task<ResultModel<PaginatedModel<AssignmentGetVM>>> GetAssignmentsForClass(long classId, QueryModel queryModel)
        {
            var query = await _assignmentRepo.GetAll().Where(m => m.SchoolClassSubject.SchoolClassId == classId)
                    .Select(x => new AssignmentGetVM
                    {
                        Id = x.Id,
                        SubjectName = x.SchoolClassSubject.Subject.Name,
                        ClassName = $"{x.SchoolClassSubject.SchoolClass.Name} {x.SchoolClassSubject.SchoolClass.ClassArm}",
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

        public async Task<ResultModel<PaginatedModel<AssignmentGetVM>>> GetAssignmentsForClassSubject(long classSubjectId, QueryModel queryModel)
        {
            var query = await _assignmentRepo.GetAll().Where(m => m.SchoolClassSubjectId == classSubjectId)
                    .Select(x => new AssignmentGetVM
                    {
                        Id = x.Id,
                        SubjectName = x.SchoolClassSubject.Subject.Name,
                        ClassName = $"{x.SchoolClassSubject.SchoolClass.Name} {x.SchoolClassSubject.SchoolClass.ClassArm}",
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
                        ClassName = $"{x.SchoolClassSubject.SchoolClass.Name} {x.SchoolClassSubject.SchoolClass.ClassArm}",
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

        public async Task<ResultModel<string>> UpdateAssignmentDueDate(AssignmentDueDateUpdateVM model)
        {
            var result = new ResultModel<string>();

            var query = await _assignmentRepo.GetAll().Where(m => m.Id == model.AssignmentId).FirstOrDefaultAsync();

            if (query == null)
            {
                result.AddError("Assignment not found.");
                return result;
            }

            if (query.CreationTime > model.NewDate)
            {
                result.AddError("New date should be greater than the assignment creation date.");
                return result;
            }

            query.DueDate = model.NewDate;

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Updated successfully";
            return result;
        }
    }
}
