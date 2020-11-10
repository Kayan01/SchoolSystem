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
        private readonly IRepository<SchoolClass, long> _schoolClassRepo;
        private readonly IRepository<Subject, long> _subjectRepo;
        private readonly IRepository<Teacher, long> _teacherRepo;
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;

        public MediaService(IUnitOfWork unitOfWork, IRepository<Media, long> mediaRepo, IDocumentService documentService, IRepository<SchoolClass, long> schoolClassRepo,
            IRepository<Subject, long> subjectRepo, IRepository<Teacher, long> teacherRepo)
        {
            _mediaRepo = mediaRepo;
            _schoolClassRepo = schoolClassRepo;
            _subjectRepo = subjectRepo;
            _teacherRepo = teacherRepo;
            _documentService = documentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultModel<PaginatedModel<MediaListVM>>> GetAllFileByClass(long classId, QueryModel queryModel)
        {
            var query = await _mediaRepo.GetAll().Where(m => m.SchoolClassId == classId)
                    .Include(m => m.Teacher).Include(m => m.Subject).Include(m => m.SchoolClass).Include(m => m.File)
                    .Select(x => (MediaListVM)x).ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            var result = new ResultModel<PaginatedModel<MediaListVM>>
            {
                Data = new PaginatedModel<MediaListVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
            return result;
        }

        public async Task<ResultModel<PaginatedModel<MediaListVM>>> GetAllFileByTeacher(long teacherId, QueryModel queryModel)
        {
            var query = await _mediaRepo.GetAll().Where(m => m.TeacherId == teacherId)
                    .Include(m => m.Teacher).Include(m => m.Subject).Include(m => m.SchoolClass).Include(m => m.File)
                    .Select(x => (MediaListVM)x).ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            var result = new ResultModel<PaginatedModel<MediaListVM>>
            {
                Data = new PaginatedModel<MediaListVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
            return result;
        }

        public async Task<ResultModel<string>> UploadLearningFile(MediaUploadVM model)
        {
            var result = new ResultModel<string>();

            var schoolClass = await _schoolClassRepo.GetAsync(model.ClassId);
            if (schoolClass == null)
            {
                result.AddError("Class not found");
                return result;
            }

            var subject = await _subjectRepo.GetAsync(model.SubjectId);
            if (subject == null)
            {
                result.AddError("Subject not found");
                return result;
            }

            var teacher = await _teacherRepo.GetAsync(model.TeacherId);
            if (teacher == null)
            {
                result.AddError("Teacher not found");
                return result;
            }


            //save file
            var file = await _documentService.TryUploadSupportingDocument(model.FileObj, Shared.Enums.DocumentType.Assignment);

            if (file != null)
            {
                result.AddError("File could not be uploaded");

                return result;
            }

            var media = new Media
            {
                File = file,
                SchoolClassId = model.ClassId,
                SubjectId = model.SubjectId,
                TeacherId = model.TeacherId,
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
