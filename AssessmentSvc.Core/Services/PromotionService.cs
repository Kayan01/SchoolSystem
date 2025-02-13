﻿using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.PubSub;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IRepository<ResultSummary, long> _resultSummaryRepo;
        private readonly IRepository<SchoolPromotionLog, long> _schoolPromotionLogRepo;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IRepository<PromotionSetup, long> _promotionSetupRepo;
        private readonly ISessionSetup _sessionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishService _publishService;

        public PromotionService(
            IRepository<ResultSummary, long> resultSummaryRepo,
            IRepository<SchoolPromotionLog, long> schoolPromotionLogRepo,
            IRepository<Student, long> studentRepo,
            IRepository<PromotionSetup, long> promotionSetupRepo,
            ISessionSetup sessionService,
            IUnitOfWork unitOfWork,
            IPublishService publishService
            )
        {
            _unitOfWork = unitOfWork;
            _resultSummaryRepo = resultSummaryRepo;
            _schoolPromotionLogRepo = schoolPromotionLogRepo;
            _studentRepo = studentRepo;
            _sessionService = sessionService;
            _promotionSetupRepo = promotionSetupRepo;
            _publishService = publishService;
        }

        public async Task<ResultModel<PromotionSharedModel>> PromoteAllStudent()
        {
            var curSessionResult = await _sessionService.GetCurrentSessionAndTerm();
            if (curSessionResult.HasError)
            {
                //return new ResultModel<string>(curSessionResult.ErrorMessages);
                return new ResultModel<PromotionSharedModel>(curSessionResult.ErrorMessages);
            }
            var currSession = curSessionResult.Data;

            var check = await _schoolPromotionLogRepo.GetAll().Where(m => m.SessionSetupId == currSession.sessionId).ToListAsync();
            if (check.Count > 0)
            {
                //return new ResultModel<string>("Promotion has already been done for this session. Promotion can not be done twice per semester.");
                return new ResultModel<PromotionSharedModel>("Promotion has already been done for this session. Promotion can not be done twice per semester.");
            }

            var promoSetup = await _promotionSetupRepo.GetAll().FirstOrDefaultAsync();
            if (promoSetup == null)
            {
                //return new ResultModel<string>(errorMessage: "Promotion setup not done.");
                return new ResultModel<PromotionSharedModel>(errorMessage: "Promotion setup not done.");
                
            }

            var allStudents = await _studentRepo.GetAll().Where(m=>m.ClassId != null).AsNoTracking().ToListAsync();

            var allResultSummary = await _resultSummaryRepo.GetAll().Where(m => m.SessionSetupId == currSession.sessionId).AsNoTracking().ToListAsync();
            var summaryGroupedByStudentId = allResultSummary.GroupBy(m => m.StudentId).ToDictionary(m => m.Key, m=> m.ToList());

            var resultNotFoundMessage =new StringBuilder("Result not found for: \n\n");
            var missingStudentResult = false;

            var promotionData = new List<StudentPromotionInfo>();

            foreach (var student in allStudents)
            {
                var results = summaryGroupedByStudentId[student.Id];

                if (results == null || results.Count<1)
                {
                    resultNotFoundMessage.Append($"{student.LastName} {student.FirstName} (Student Id : {student.Id})\n");
                    missingStudentResult = true;
                    continue;
                }

                if (!missingStudentResult)
                {
                    var allAverage = results.Average(m => m.ResultTotalAverage);

                    promotionData.Add(new StudentPromotionInfo()
                    {
                        Average = allAverage,
                        PassedCutoff = allAverage >= promoSetup.PromotionScore,
                        StudentId = student.Id
                    });
                }
            }

            if (missingStudentResult)
            {
                //return new ResultModel<string>(errorMessage: $"{resultNotFoundMessage.ToString()} \n Please add student's result or update=");
                return new ResultModel<PromotionSharedModel>(errorMessage: $"{resultNotFoundMessage.ToString()} \n Please add student's result or update=");
            }
             var publishPayload = new PromotionSharedModel
            {
                CutOff = promoSetup.PromotionScore,
                MaxRepeats = promoSetup.MaxRepeat,
                SessionId = currSession.sessionId,
                SessionName = currSession.SessionName,
                StudentPromotionInfoList = promotionData,
                TenantId = allStudents.First().TenantId,
                IsAutomaticPromotion = promoSetup.PromotionType == Enumeration.PromotionType.Automatic
            };

            //await _publishService.PublishMessage(Topics.Promotion, BusMessageTypes.PROMOTION, publishPayload);
            var log = new SchoolPromotionLog()
            {
                Payload = JsonConvert.SerializeObject(publishPayload),
                SessionSetupId = currSession.sessionId,
            };

            _schoolPromotionLogRepo.Insert(log);
            
            await _unitOfWork.SaveChangesAsync();

            //Broadcast nessage to Auth handler for promotions.
           // await _publishService.PublishMessage(Topics.Promotion, BusMessageTypes.PROMOTION, publishPayload);

            return new ResultModel<PromotionSharedModel>(data: publishPayload);
        }
    }
}
