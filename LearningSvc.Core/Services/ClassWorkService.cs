using LearningSvc.Core.Models;
using LearningSvc.Core.Models.Files;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.ClassWork;
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
    public class ClassWorkService : IClassWorkService
    {
        private readonly IRepository<Classwork, long> _classworkRepo;
        private readonly IRepository<SchoolClass, long> _schoolclassRepo;
        private readonly IRepository<Subject, long> _subjectRepo;
        private readonly IRepository<Teacher, long> _teacherRepo;
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;

        public ClassWorkService(IUnitOfWork unitOfWork, IRepository<Classwork, long> classworkRepo, IDocumentService documentService, IRepository<SchoolClass, long> schoolclassRepo, 
            IRepository<Subject, long> subjectRepo, IRepository<Teacher, long> teacherRepo)
        {
            _classworkRepo = classworkRepo;
            _schoolclassRepo = schoolclassRepo;
            _subjectRepo = subjectRepo;
            _teacherRepo = teacherRepo;
            _documentService = documentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultModel<PaginatedList<ClassWorkListVM>>> GetAllFileByClass(long classId, int pagenumber, int pagesize)
        {
            var query = _classworkRepo.GetAll().Where(m => m.SchoolClassId == classId)
                    .Include(m => m.Teacher).Include(m => m.Subject).Include(m => m.SchoolClass).Include(m => m.File)
                    .Select(x => (ClassWorkListVM)x);

            var files = await PaginatedList<ClassWorkListVM>.CreateAsync(query, pagenumber, pagesize);

            var result = new ResultModel<PaginatedList<ClassWorkListVM>>
            {
                Data = files
            };
            return result;
        }

        public async Task<ResultModel<PaginatedList<ClassWorkListVM>>> GetAllFileByTeacher(long teacherId, int pagenumber, int pagesize)
        {
            var query = _classworkRepo.GetAll().Where(m => m.TeacherId == teacherId)
                    .Include(m => m.Teacher).Include(m => m.Subject).Include(m => m.SchoolClass).Include(m => m.File)
                    .Select(x => (ClassWorkListVM)x);

            var files = await PaginatedList<ClassWorkListVM>.CreateAsync(query, pagenumber, pagesize);

            var result = new ResultModel<PaginatedList<ClassWorkListVM>>
            {
                Data = files
            };
            return result;
        }

        public async Task<ResultModel<string>> UploadLearningFile(ClassWorkUploadVM model)
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

            var f = new Classwork
            {
                File = files[0],
                SchoolClassId = model.ClassId,
                SubjectId = model.SubjectId,
                TeacherId = model.TeacherId,
            };

            await _classworkRepo.InsertAsync(f);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<string>> DeleteLearningFile(long fileId)
        {
            var result = new ResultModel<string>();

            var file = await _classworkRepo.GetAsync(fileId);

            if (file == null)
            {
                result.AddError("File does not exist.");
                return result;
            }

            await _classworkRepo.DeleteAsync(file);
            await _unitOfWork.SaveChangesAsync();

            result.Data = "Deleted successfully";
            return result;
        }
    }
}
