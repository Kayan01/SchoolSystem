using LearningSvc.Core.Models;
using LearningSvc.Core.Models.Files;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.LessonNote;
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
using Shared.Pagination;
using IPagedList;

namespace LearningSvc.Core.Services
{
    public class LessonNoteService: ILessonNoteService
    {
        private readonly IRepository<LessonNote, long> _lessonnoteRepo;
        private readonly IRepository<SchoolClassSubject, long> _schoolClassSubjectRepo;
        private readonly IRepository<Teacher, long> _teacherRepo;
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;

        public LessonNoteService(IUnitOfWork unitOfWork, IRepository<LessonNote, long> lessonnoteRepo, IDocumentService documentService,
            IRepository<SchoolClassSubject, long> schoolClassSubjectRepo, IRepository<Teacher, long> teacherRepo)
        {
            _lessonnoteRepo = lessonnoteRepo;
            _schoolClassSubjectRepo = schoolClassSubjectRepo;
            _teacherRepo = teacherRepo;
            _documentService = documentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultModel<PaginatedModel<LessonNoteListVM>>> GetAllFileByClass(long classId, QueryModel queryModel)
        {
            var query = await _lessonnoteRepo.GetAll().Where(m => m.SchoolClassSubject.SchoolClassId == classId)
                    .Select(x => new LessonNoteListVM
                    {
                        Id = x.Id,
                        Name = x.File.Name,
                        ClassName = $"{x.SchoolClassSubject.SchoolClass.Name} {x.SchoolClassSubject.SchoolClass.ClassArm}",
                        CreationDate = x.CreationTime,
                        FileId = x.FileUploadId,
                        SubjectName = x.SchoolClassSubject.Subject.Name,
                        TeacherName = $"{x.Teacher.FirstName} {x.Teacher.LastName}",
                    }).ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            var result = new ResultModel<PaginatedModel<LessonNoteListVM>>
            {
                Data = new PaginatedModel<LessonNoteListVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
            return result;
        }

        public async Task<ResultModel<PaginatedModel<LessonNoteListVM>>> GetAllFileByTeacher(long currentUserId, QueryModel queryModel)
        {
            var teacher = await _teacherRepo.GetAll().Where(m => m.UserId == currentUserId).FirstOrDefaultAsync();
            if (teacher == null)
            {
                var r = new ResultModel<PaginatedModel<LessonNoteListVM>>();
                r.AddError("Current user is not a valid Teacher");
                return r;
            }

            var query = await _lessonnoteRepo.GetAll().Where(m => m.TeacherId == teacher.Id)
                    .Select(x => new LessonNoteListVM
                    {
                        Id = x.Id,
                        Name = x.File.Name,
                        ClassName = $"{x.SchoolClassSubject.SchoolClass.Name} {x.SchoolClassSubject.SchoolClass.ClassArm}",
                        CreationDate = x.CreationTime,
                        FileId = x.FileUploadId,
                        SubjectName = x.SchoolClassSubject.Subject.Name,
                        TeacherName = $"{x.Teacher.FirstName} {x.Teacher.LastName}",
                    }).ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            var result = new ResultModel<PaginatedModel<LessonNoteListVM>>
            {
                Data = new PaginatedModel<LessonNoteListVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
            return result;
        }

        public async Task<ResultModel<string>> UploadLearningFile(LessonNoteUploadVM model, long currentUserId)
        {
            var result = new ResultModel<string>();

            var schoolClassSubject = await _schoolClassSubjectRepo.GetAsync(model.ClassSubjectId);
            if (schoolClassSubject == null)
            {
                result.AddError("Class subject was not found");
                return result;
            }

            var teacher = await _teacherRepo.GetAll().Where(m => m.UserId == currentUserId).FirstOrDefaultAsync();
            if (teacher == null)
            {
                result.AddError("Current user is not a valid Teacher");
                return result;
            }

            //save file
            var file = await _documentService.TryUploadSupportingDocument(model.FileObj, Shared.Enums.DocumentType.Assignment);

            if (file == null)
            {
                result.AddError("File could not be uploaded");

                return result;
            }

            var lessonNote = new LessonNote
            {
                File = file,
                SchoolClassSubjectId = model.ClassSubjectId,
                TeacherId = teacher.Id,
                OptionalComment = model.Comment
            };

            await _lessonnoteRepo.InsertAsync(lessonNote);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<string>> DeleteLessonNote(long id)
        {
            var result = new ResultModel<string>();

            var lessonNote = await _lessonnoteRepo.GetAsync(id);

            if (lessonNote == null)
            {
                result.AddError("File does not exist.");
                return result;
            }

            await _lessonnoteRepo.DeleteAsync(lessonNote);
            await _unitOfWork.SaveChangesAsync();

            result.Data = "Deleted successfully";
            return result;
        }
    }
}
