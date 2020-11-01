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
        private readonly IRepository<Classwork, long> _classWorkRepo;
        private readonly IRepository<SchoolClass, long> _schoolClassRepo;
        private readonly IRepository<Subject, long> _subjectRepo;
        private readonly IRepository<Teacher, long> _teacherRepo;
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;

        public ClassWorkService(IUnitOfWork unitOfWork, IRepository<Classwork, long> classWorkRepo, IDocumentService documentService, IRepository<SchoolClass, long> schoolClassRepo, 
            IRepository<Subject, long> subjectRepo, IRepository<Teacher, long> teacherRepo)
        {
            _classWorkRepo = classWorkRepo;
            _schoolClassRepo = schoolClassRepo;
            _subjectRepo = subjectRepo;
            _teacherRepo = teacherRepo;
            _documentService = documentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultModel<PaginatedList<ClassWorkListVM>>> GetAllFileByClass(long classId, int pagenumber, int pagesize)
        {
            var query = _classWorkRepo.GetAll().Where(m => m.SchoolClassId == classId)
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
            var query = _classWorkRepo.GetAll().Where(m => m.TeacherId == teacherId)
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

            var classwork = new Classwork
            {
                File = file,
                SchoolClassId = model.ClassId,
                SubjectId = model.SubjectId,
                TeacherId = model.TeacherId,
            };

            await _classWorkRepo.InsertAsync(classwork);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<string>> DeleteClassNote(long id)
        {
            var result = new ResultModel<string>();

            var file = await _classWorkRepo.GetAsync(id);

            if (file == null)
            {
                result.AddError("File does not exist.");
                return result;
            }

            await _classWorkRepo.DeleteAsync(file);
            await _unitOfWork.SaveChangesAsync();

            result.Data = "Deleted successfully";
            return result;
        }
    }
}
