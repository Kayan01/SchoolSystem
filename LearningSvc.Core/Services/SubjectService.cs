﻿using LearningSvc.Core.Models;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.Subject;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Pagination;
using IPagedList;
using Shared.PubSub;
using LearningSvc.Core.Context;

namespace LearningSvc.Core.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IRepository<Subject, long> _subjectRepo;
        private readonly IRepository<SchoolClass, long> _schoolClassRepo;
        private readonly IClassSubjectService _classSubjectService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishService _publishService;
        private readonly AppDbContext _context;

        public SubjectService(IUnitOfWork unitOfWork, 
            IRepository<Subject, long> subjectRepo, 
            IRepository<SchoolClass, long> schoolClassRepo, 
            IClassSubjectService classSubjectService,
            IPublishService publishService,
            AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _subjectRepo = subjectRepo;
            _schoolClassRepo = schoolClassRepo;
            _classSubjectService = classSubjectService;
            _publishService = publishService;
            _context = context;
        }

        public async Task<ResultModel<SubjectVM>> AddSubject(SubjectInsertVM model)
        {
            var result = new ResultModel<SubjectVM>();

            var check = await _subjectRepo.GetAll().Where(m => m.Name.ToLower() == model.Name.ToLower()).FirstOrDefaultAsync();
            if (check != null)
            {
                result.AddError("Subject already Exists.");
                return result;
            }

            //create class session
            var subject = new Subject
            {
                //Todo : Add more fields
                Name = model.Name.ToLower(),
                IsActive = model.IsActive
            };

            _subjectRepo.Insert(subject);
            await _unitOfWork.SaveChangesAsync();

            if (model.ClassIds != null && model.ClassIds.Length > 0)
            {
                var classes = await _schoolClassRepo.GetAll().Where(x => model.ClassIds.Contains(x.Id)).Select(m=>m.Id).ToListAsync();

                if (classes.Count>0)
                {
                    await _classSubjectService.AddClassesToSubject(new ViewModels.ClassSubject.ClassesToSubjectInsertVM()
                    {
                        SubjectId = subject.Id,
                        ClassIds = classes.ToArray(),
                    });
                }
            }

            var subjectSharedModel = new SubjectSharedModel
            {
                Id = subject.Id,
                Name = subject.Name,
                TenantId = subject.TenantId,
                IsActive = subject.IsActive,
            };


            await _publishService.PublishMessage(Topics.Subject, BusMessageTypes.SUBJECT, subjectSharedModel);

            result.Data = new SubjectVM() { Id = subject.Id, Name = model.Name.ToLower(), IsActive = model.IsActive};
            return result;
        }

        public async Task<ResultModel<SubjectVM>> UpdateSubject(SubjectUpdateVM model)
        {
            var result = new ResultModel<SubjectVM>();

            var subject = await _subjectRepo.GetAll().Where(m => m.Id == model.Id).FirstOrDefaultAsync();
            if (subject is null)
            {
                result.AddError("Subject not found.");
                return result;
            }

            subject.Name = model.Name;
            subject.IsActive = model.IsActive;

            await  _subjectRepo.UpdateAsync(subject);
            await _unitOfWork.SaveChangesAsync();

            var subjectSharedModel = new SubjectSharedModel
            {
                Id = subject.Id,
                Name = subject.Name,
                TenantId = subject.TenantId,
                IsActive = subject.IsActive,
            };


            await _publishService.PublishMessage(Topics.Subject, BusMessageTypes.SUBJECT_UPDATE, subjectSharedModel);

            result.Data = new SubjectVM() { Id = subject.Id, Name = model.Name.ToLower(), IsActive = model.IsActive };
            return result;
        }


        public async Task<ResultModel<PaginatedModel<SubjectVM>>> GetAllSubjects(QueryModel queryModel)
        {
            var query = await _subjectRepo.GetAll().Select(x => (SubjectVM)x).ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            var result = new ResultModel<PaginatedModel<SubjectVM>>
            {
                Data = new PaginatedModel<SubjectVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
            return result;
        }


        public async Task<ResultModel<List<SubjectVM>>> GetAllSubjects()
        {
            var result = new ResultModel<List<SubjectVM>>();

            var data = await _subjectRepo.GetAll().Select(x => new SubjectVM
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive
            }).ToListAsync();

            result.Data = data;
            return result;
        }


        public async Task<ResultModel<string>> RemoveSubject(long SubjectId)
        {
            var resultModel = new ResultModel<string>();

            var query = await _subjectRepo.GetAllIncluding(x => x.SchoolClassSubjects).Where(x => x.Id == SubjectId).FirstOrDefaultAsync();
            if (query == null)
            {
                resultModel.AddError("Subject with provided Id does not exist");
                return resultModel;
            }

            if (query.SchoolClassSubjects.Count > 0) 
            {
                resultModel.AddError("Cannot delete this subject becasue it has  been attached to a class");
                return resultModel;
            }

            _subjectRepo.Delete(SubjectId);
            await _unitOfWork.SaveChangesAsync();

            SubjectSharedModel subjectSharedModel = new SubjectSharedModel()
            {
                Id = query.Id,
                Name = query.Name,
                TenantId = query.TenantId,
                IsActive = query.IsActive,
            };

            await _publishService.PublishMessage(Topics.Subject, BusMessageTypes.SUBJECT_DELETE, subjectSharedModel);


            resultModel.Message = "Subject removed Successfully";
            resultModel.Data = "Delete Action taken Successful";

            return resultModel;
        }

    }
}
