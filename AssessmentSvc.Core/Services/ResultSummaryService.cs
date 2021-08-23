using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.ViewModels.Result;
using AssessmentSvc.Core.ViewModels.SessionSetup;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Services
{
    public class ResultSummaryService : IResultSummaryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ResultSummary, long> _resultSummaryRepo;
        private readonly IRepository<Result, long> _resultRepo;
        private readonly IRepository<SchoolClass, long> _schoolClassRepo;
        private readonly ISessionSetup _sessionService;
        private readonly IResultService _resultService;
        private readonly IGradeSetupService _gradeService;
        private readonly IAssessmentSetupService _assessmentSetupService;
        private readonly IStudentService _studentService;

        public ResultSummaryService(
            IRepository<ResultSummary, long> resultSummaryRepo,
            IRepository<Result, long> resultRepo,
            IRepository<SchoolClass, long> schoolClassRepo,
            ISessionSetup sessionService,
            IResultService resultService,
            IGradeSetupService gradeService,
            IStudentService studentService,
            IAssessmentSetupService assessmentSetupService,
        IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _resultService = resultService;
            _sessionService = sessionService;
            _resultSummaryRepo = resultSummaryRepo;
            _resultRepo = resultRepo;
            _schoolClassRepo = schoolClassRepo;
            _gradeService = gradeService;
            _assessmentSetupService = assessmentSetupService;
            _studentService = studentService;
        }

        public async Task<ResultModel<List<StudentResultSummaryVM>>> CalculateResultSummaries()
        {//get grade setup for school
            var gradeSetupResult = await _gradeService.GetAllGradeForSchoolSetup();

            if (gradeSetupResult.HasError || gradeSetupResult.Data.Count < 1)
            {
                return new ResultModel<List<StudentResultSummaryVM>>("Grade has not been setup");
            }

            var sessionAndTermResult = await _sessionService.GetCurrentSessionAndTerm();

            if (sessionAndTermResult.HasError)
            {
                return new ResultModel<List<StudentResultSummaryVM>>(sessionAndTermResult.ErrorMessages);
            }

            var currSessionAndTerm = sessionAndTermResult.Data;

            var classes = await _schoolClassRepo.GetAll()
                .Include(m => m.SchoolClassSubjects)
                .ToListAsync();

            if (classes.Count < 1)
            {
                return new ResultModel<List<StudentResultSummaryVM>>("No Class was found.");
            }

            var studentsApprovedResults = await _resultRepo.GetAll()
                .Where(x => x.SessionSetupId == currSessionAndTerm.sessionId &&
                    x.TermSequenceNumber == currSessionAndTerm.TermSequence &&
                    x.ApprovedResult.ClassTeacherApprovalStatus == Enumeration.ApprovalStatus.Approved &&
                    x.ApprovedResult.HeadTeacherApprovedStatus == Enumeration.ApprovalStatus.Approved)
                .Include(x => x.Subject).AsNoTracking()
                .ToListAsync();

            if (studentsApprovedResults.Count < 1)
            {
                return new ResultModel<List<StudentResultSummaryVM>>(errorMessage: "No Approved result found in current term and session");
            }
            var uniqueStudentsResults = studentsApprovedResults.GroupBy(x => x.StudentId).ToList();


            var resultSummaries = await _resultSummaryRepo.GetAll().Where(m => 
                m.SessionSetupId == currSessionAndTerm.sessionId && 
                m.TermSequenceNumber == currSessionAndTerm.TermSequence
            ).ToListAsync();

            resultSummaries ??= new List<ResultSummary>();

            var newSummaries = new List<ResultSummary>();

            foreach (var studentApprovedResults in uniqueStudentsResults)
            {
                var SubjectIds = new List<long>();
                var totalScore = 0d;

                foreach (var result in studentApprovedResults)
                {
                    SubjectIds.Add(result.SubjectId);
                    totalScore += result.Scores.Sum(m => m.StudentScore);
                }

                var distinctSubjectIds = SubjectIds.Distinct();
                var oneResult = studentApprovedResults.First();

                var studentSummary = resultSummaries.FirstOrDefault(m => m.StudentId == studentApprovedResults.Key);

                if (studentSummary == null)
                {
                    studentSummary = new ResultSummary()
                    {
                        ResultApproved = distinctSubjectIds.Count() == classes.FirstOrDefault(m => m.Id == oneResult.SchoolClassId).SchoolClassSubjects.Count,
                        ResultTotalAverage = totalScore / distinctSubjectIds.Count(),
                        ResultTotal = totalScore,
                        SchoolClassId = oneResult.SchoolClassId,
                        SessionSetupId = currSessionAndTerm.sessionId,
                        StudentId = studentApprovedResults.Key,
                        TermSequenceNumber = currSessionAndTerm.TermSequence
                    };
                }
                else
                {
                    studentSummary.ResultApproved = distinctSubjectIds.Count() == classes.FirstOrDefault(m => m.Id == oneResult.SchoolClassId).SchoolClassSubjects.Count;
                    studentSummary.ResultTotalAverage = totalScore / distinctSubjectIds.Count();
                    studentSummary.ResultTotal = totalScore;
                    studentSummary.SchoolClassId = oneResult.SchoolClassId;
                    studentSummary.SessionSetupId = currSessionAndTerm.sessionId;
                }

                newSummaries.Add(studentSummary);
            }

            var rtnData = new List<StudentResultSummaryVM>();

            //calculate position by class
            var resultByClass = newSummaries.GroupBy(m => m.SchoolClassId);
            foreach (var classSummaries in resultByClass)
            {
                //calculate position
                var orderedResults = classSummaries.OrderByDescending(x => x.ResultTotalAverage).ToList();
                for (int i = 0; i < orderedResults.Count; i++)
                {
                    orderedResults[i].ClassPosition = i + 1;
                    //insert or update to database
                    _resultSummaryRepo.InsertOrUpdate(orderedResults[i]);
                    rtnData.Add(new StudentResultSummaryVM()
                    {
                        Total = orderedResults[i].ResultTotal,
                        Average = orderedResults[i].ResultTotalAverage,
                        isApproved = orderedResults[i].ResultApproved,
                        StudentId = orderedResults[i].StudentId
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<List<StudentResultSummaryVM>>(rtnData);
        }

        public async Task<ResultModel<List<ResultSummary>>> GetResultSummaries(long? curSessionId = null, int? termSequenceNumber = null)
        {
            var sessionAndTermResult = new ResultModel<CurrentSessionAndTermVM>();

            if (curSessionId != null && termSequenceNumber != null)
            {
                sessionAndTermResult = await _sessionService.GetSessionAndTerm(curSessionId.Value, termSequenceNumber.Value);
            }
            else
            {
                sessionAndTermResult = await _sessionService.GetCurrentSessionAndTerm();
            }

            if (sessionAndTermResult.HasError)
            {
                return new ResultModel<List<ResultSummary>>(sessionAndTermResult.ErrorMessages);
            }

            var currSessionAndTerm = sessionAndTermResult.Data;

            var resultSummaries = await _resultSummaryRepo.GetAll().Where(m =>
                m.SessionSetupId == currSessionAndTerm.sessionId &&
                m.TermSequenceNumber == currSessionAndTerm.TermSequence
            ).AsNoTracking().ToListAsync();

            return new ResultModel<List<ResultSummary>>(resultSummaries);
        }

    }
}
