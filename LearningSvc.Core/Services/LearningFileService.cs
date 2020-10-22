using LearningSvc.Core.Models;
using LearningSvc.Core.Services.Interfaces;
using LearningSvc.Core.ViewModels.LearningFiles;
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
    public class LearningFileService : ILearningFileService
    {
        private readonly IRepository<LearningFile, long> _fileRepo;
        private readonly IRepository<Assignment, long> _assignmentRepo;
        private readonly IRepository<SchoolClass, long> _cRepo;
        private readonly IRepository<Subject, long> _sRepo;
        private readonly IRepository<Teacher, long> _tRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentService _documentService;

        public LearningFileService(IUnitOfWork unitOfWork, IRepository<LearningFile, long> lRepo, IDocumentService documentService, IRepository<Assignment, long> assignmentRepo,
            IRepository<SchoolClass, long> cRepo, IRepository<Subject, long> sRepo, IRepository<Teacher, long> tRepo)
        {
            _unitOfWork = unitOfWork;
            _fileRepo = lRepo;
            _assignmentRepo = assignmentRepo;
            _cRepo = cRepo;
            _sRepo = sRepo;
            _tRepo = tRepo;
            _documentService = documentService;
        }

        public async Task<ResultModel<string>> DeleteLearningFile(long fileId)
        {
            var result = new ResultModel<string>();

            var file = await _fileRepo.GetAsync(fileId);

            if (file == null)
            {
                result.AddError("File does not exist.");
                return result;
            }

            if (file.FileType == Enumerations.LearningFileType.Assignment)
            {
                result.AddError("Assignment files can not be deleted from here.");
                return result;
            }

            await _fileRepo.DeleteAsync(file);
            await _unitOfWork.SaveChangesAsync();

            result.Data = "Deleted successfully";
            return result;
        }

        public async Task<ResultModel<List<LearningFileListVM>>> GetAllFileByClass(long classId)
        {
            var files = await _fileRepo.GetAll().Where(m => m.TeacherId == classId)
                    .Include(m => m.Teacher).Include(m => m.Subject).Include(m => m.SchoolClass).Include(m => m.File)
                    .Select(x => (LearningFileListVM)x).ToListAsync();

            files.AddRange(
                await _assignmentRepo.GetAll().Where(m => m.TeacherId == classId)
                    .Include(m => m.Teacher).Include(m => m.Subject).Include(m => m.SchoolClass).Include(m => m.Attachment)
                    .Select(x => (LearningFileListVM)x).ToListAsync()
                );

            var result = new ResultModel<List<LearningFileListVM>>
            {
                Data = files.OrderByDescending(m => m.CreationDate).ToList()
            };
            return result;
        }

        public async Task<ResultModel<List<LearningFileListVM>>> GetAllFileByTeacher(long teacherId)
        {
            var files = await _fileRepo.GetAll().Where(m => m.TeacherId == teacherId)
                    .Include(m => m.Teacher).Include(m => m.Subject).Include(m => m.SchoolClass).Include(m => m.File)
                    .Select(x => (LearningFileListVM)x).ToListAsync();

            files.AddRange(
                await _assignmentRepo.GetAll().Where(m => m.TeacherId == teacherId)
                    .Include(m => m.Teacher).Include(m => m.Subject).Include(m => m.SchoolClass).Include(m => m.Attachment)
                    .Select(x => (LearningFileListVM)x).ToListAsync()
                );

            var result = new ResultModel<List<LearningFileListVM>>
            {
                Data = files.OrderByDescending(m => m.CreationDate).ToList()
            };
            return result;
        }

        public async Task<ResultModel<string>> UploadLearningFile(LearningFileUploadVM model)
        {
            var result = new ResultModel<string>();

            if (model.FileType == Enumerations.LearningFileType.Assignment)
            {
                result.AddError("Assignment files should be uploaded with assignment end-point.");
                return result;
            }

            var c = await _cRepo.GetAsync(model.ClassId);
            if (c == null)
            {
                result.AddError("Class not found");
                return result;
            }

            var s = await _sRepo.GetAsync(model.SubjectId);
            if (s == null)
            {
                result.AddError("Subject not found");
                return result;
            }

            var t = await _tRepo.GetAsync(model.TeacherId);
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

            var f = new LearningFile
            {
                File = files[0],
                FileType = model.FileType,
                SchoolClassId = model.ClassId,
                SubjectId = model.SubjectId,
                TeacherId = model.TeacherId,
            };

            await _fileRepo.InsertAsync(f);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }
    }
}
