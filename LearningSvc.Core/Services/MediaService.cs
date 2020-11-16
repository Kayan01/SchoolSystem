using LearningSvc.Core.Models;
using LearningSvc.Core.Models.Files;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.Media;
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
    public class MediaService : IMediaService
    {
        private readonly IRepository<Media, long> _mediaRepo;
        private readonly IRepository<SchoolClassSubject, long> _schoolClassSubjectRepo;
        private readonly IRepository<Teacher, long> _teacherRepo;
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;

        public MediaService(IUnitOfWork unitOfWork, IRepository<Media, long> mediaRepo, IDocumentService documentService,
            IRepository<SchoolClassSubject, long> schoolClassSubjectRepo, IRepository<Teacher, long> teacherRepo)
        {
            _mediaRepo = mediaRepo;
            _schoolClassSubjectRepo = schoolClassSubjectRepo;
            _teacherRepo = teacherRepo;
            _documentService = documentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultModel<PaginatedModel<MediaListVM>>> GetAllFileByClass(long classId, QueryModel queryModel)
        {
            var query = await _mediaRepo.GetAll().Where(m => m.SchoolClassSubject.SchoolClassId == classId)
                    .Select(x => new MediaListVM
                    {
                        Id = x.Id,
                        Name = x.File.Name,
                        ClassName = $"{x.SchoolClassSubject.SchoolClass.Name} {x.SchoolClassSubject.SchoolClass.ClassArm}",
                        CreationDate = x.CreationTime,
                        FileId = x.FileUploadId,
                        SubjectName = x.SchoolClassSubject.Subject.Name,
                        TeacherName = $"{x.Teacher.FirstName} {x.Teacher.LastName}",
                    })
                    .ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            var result = new ResultModel<PaginatedModel<MediaListVM>>
            {
                Data = new PaginatedModel<MediaListVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
            return result;
        }

        public async Task<ResultModel<PaginatedModel<MediaListVM>>> GetAllFileByTeacher(long currentUserId, QueryModel queryModel)
        {
            var teacher = await _teacherRepo.GetAll().Where(m => m.UserId == currentUserId).FirstOrDefaultAsync();
            if (teacher == null)
            {
                var r = new ResultModel<PaginatedModel<MediaListVM>>();
                r.AddError("Current user is not a valid Teacher");
                return r;
            }

            var query = await _mediaRepo.GetAll().Where(m => m.TeacherId == teacher.Id)
                    .Select(x => new MediaListVM
                    {
                        Id = x.Id,
                        Name = x.File.Name,
                        ClassName = $"{x.SchoolClassSubject.SchoolClass.Name} {x.SchoolClassSubject.SchoolClass.ClassArm}",
                        CreationDate = x.CreationTime,
                        FileId = x.FileUploadId,
                        SubjectName = x.SchoolClassSubject.Subject.Name,
                        TeacherName = $"{x.Teacher.FirstName} {x.Teacher.LastName}",
                    })
                    .ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            var result = new ResultModel<PaginatedModel<MediaListVM>>
            {
                Data = new PaginatedModel<MediaListVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
            return result;
        }

        public async Task<ResultModel<string>> UploadLearningFile(MediaUploadVM model, long currentUserId)
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

            var media = new Media
            {
                File = file,
                SchoolClassSubjectId = model.ClassSubjectId,
                TeacherId = teacher.Id,
                OptionalComment = model.Comment
            };

            await _mediaRepo.InsertAsync(media);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<string>> DeleteMedia(long id)
        {
            var result = new ResultModel<string>();

            var media = await _mediaRepo.GetAsync(id);

            if (media == null)
            {
                result.AddError("File does not exist.");
                return result;
            }

            await _mediaRepo.DeleteAsync(media);
            await _unitOfWork.SaveChangesAsync();

            result.Data = "Deleted successfully";
            return result;
        }
    }
}
