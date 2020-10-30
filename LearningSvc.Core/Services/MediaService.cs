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

namespace LearningSvc.Core.Services
{
    public class MediaService : IMediaService
    {
        private readonly IRepository<Media, long> _mediaRepo;
        private readonly IRepository<SchoolClass, long> _schoolclassRepo;
        private readonly IRepository<Subject, long> _subjectRepo;
        private readonly IRepository<Teacher, long> _teacherRepo;
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;

        public MediaService(IUnitOfWork unitOfWork, IRepository<Media, long> mediaRepo, IDocumentService documentService, IRepository<SchoolClass, long> schoolclassRepo,
            IRepository<Subject, long> subjectRepo, IRepository<Teacher, long> teacherRepo)
        {
            _mediaRepo = mediaRepo;
            _schoolclassRepo = schoolclassRepo;
            _subjectRepo = subjectRepo;
            _teacherRepo = teacherRepo;
            _documentService = documentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultModel<PaginatedList<MediaListVM>>> GetAllFileByClass(long classId, int pagenumber, int pagesize)
        {
            var query = _mediaRepo.GetAll().Where(m => m.SchoolClassId == classId)
                    .Include(m => m.Teacher).Include(m => m.Subject).Include(m => m.SchoolClass).Include(m => m.File)
                    .Select(x => (MediaListVM)x);

            var files = await PaginatedList<MediaListVM>.CreateAsync(query, pagenumber, pagesize);

            var result = new ResultModel<PaginatedList<MediaListVM>>
            {
                Data = files
            };
            return result;
        }

        public async Task<ResultModel<PaginatedList<MediaListVM>>> GetAllFileByTeacher(long teacherId, int pagenumber, int pagesize)
        {
            var query = _mediaRepo.GetAll().Where(m => m.TeacherId == teacherId)
                    .Include(m => m.Teacher).Include(m => m.Subject).Include(m => m.SchoolClass).Include(m => m.File)
                    .Select(x => (MediaListVM)x);

            var files = await PaginatedList<MediaListVM>.CreateAsync(query, pagenumber, pagesize);

            var result = new ResultModel<PaginatedList<MediaListVM>>
            {
                Data = files
            };
            return result;
        }

        public async Task<ResultModel<string>> UploadLearningFile(MediaUploadVM model)
        {
            var result = new ResultModel<string>();

            var c = await _schoolclassRepo.GetAsync(model.ClassId);
            if (c == null)
            {
                result.AddError("Class not found");
                return result;
            }

            var s = await _subjectRepo.GetAsync(model.SubjectId);
            if (s == null)
            {
                result.AddError("Subject not found");
                return result;
            }

            var t = await _teacherRepo.GetAsync(model.TeacherId);
            if (t == null)
            {
                result.AddError("Teacher not found");
                return result;
            }


            //save logo
            var files = _documentService.TryUploadSupportingDocuments(new List<DocumentVM> { model.FileObj });

            if (files.Count() != 1)
            {
                result.AddError("Some files could not be uploaded");

                return result;
            }

            var f = new Media
            {
                File = files[0],
                SchoolClassId = model.ClassId,
                SubjectId = model.SubjectId,
                TeacherId = model.TeacherId,
            };

            await _mediaRepo.InsertAsync(f);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<string>> DeleteLearningFile(long fileId)
        {
            var result = new ResultModel<string>();

            var file = await _mediaRepo.GetAsync(fileId);

            if (file == null)
            {
                result.AddError("File does not exist.");
                return result;
            }

            await _mediaRepo.DeleteAsync(file);
            await _unitOfWork.SaveChangesAsync();

            result.Data = "Deleted successfully";
            return result;
        }
    }
}
