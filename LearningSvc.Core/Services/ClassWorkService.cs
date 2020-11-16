﻿using LearningSvc.Core.Models;
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
using Shared.Pagination;
using IPagedList;

namespace LearningSvc.Core.Services
{
    public class ClassWorkService : IClassWorkService
    {
        private readonly IRepository<Classwork, long> _classWorkRepo;
        private readonly IRepository<SchoolClassSubject, long> _schoolClassSubjectRepo;
        private readonly IRepository<Teacher, long> _teacherRepo;
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;

        public ClassWorkService(IUnitOfWork unitOfWork, IRepository<Classwork, long> classWorkRepo, IDocumentService documentService,
            IRepository<SchoolClassSubject, long> schoolClassSubjectRepo, IRepository<Teacher, long> teacherRepo)
        {
            _classWorkRepo = classWorkRepo;
            _schoolClassSubjectRepo = schoolClassSubjectRepo;
            _teacherRepo = teacherRepo;
            _documentService = documentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultModel<PaginatedModel<ClassWorkListVM>>> GetAllFileByClass(long classId, QueryModel queryModel)
        {
            var query = await _classWorkRepo.GetAll().Where(m => m.SchoolClassSubject.SchoolClassId == classId)
                    .Select(x => new ClassWorkListVM
                    {
                        Id = x.Id,
                        Name = x.File.Name,
                        ClassName = $"{x.SchoolClassSubject.SchoolClass.Name} {x.SchoolClassSubject.SchoolClass.ClassArm}",
                        CreationDate = x.CreationTime,
                        FileId = x.FileUploadId,
                        SubjectName = x.SchoolClassSubject.Subject.Name,
                        TeacherName = $"{x.Teacher.FirstName} {x.Teacher.LastName}",
                    }).ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            var result = new ResultModel<PaginatedModel<ClassWorkListVM>>
            {
                Data = new PaginatedModel<ClassWorkListVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
            return result;
        }

        public async Task<ResultModel<PaginatedModel<ClassWorkListVM>>> GetAllFileByTeacher(long teacherId, QueryModel queryModel)
        {
            var query = await _classWorkRepo.GetAll().Where(m => m.TeacherId == teacherId)
                    .Select(x => new ClassWorkListVM
                    {
                        Id = x.Id,
                        Name = x.File.Name,
                        ClassName = $"{x.SchoolClassSubject.SchoolClass.Name} {x.SchoolClassSubject.SchoolClass.ClassArm}",
                        CreationDate = x.CreationTime,
                        FileId = x.FileUploadId,
                        SubjectName = x.SchoolClassSubject.Subject.Name,
                        TeacherName = $"{x.Teacher.FirstName} {x.Teacher.LastName}",
                    }).ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            var result = new ResultModel<PaginatedModel<ClassWorkListVM>>
            {
                Data = new PaginatedModel<ClassWorkListVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
            return result;
        }

        public async Task<ResultModel<string>> UploadLearningFile(ClassWorkUploadVM model)
        {
            var result = new ResultModel<string>();

            var schoolClassSubject = await _schoolClassSubjectRepo.GetAsync(model.ClassSubjectId);
            if (schoolClassSubject == null)
            {
                result.AddError("Class subject was not found");
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

            if (file == null)
            {
                result.AddError("File could not be uploaded");

                return result;
            }

            var classwork = new Classwork
            {
                File = file,
                SchoolClassSubjectId = model.ClassSubjectId,
                TeacherId = model.TeacherId,
                OptionalComment = model.Comment,
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
