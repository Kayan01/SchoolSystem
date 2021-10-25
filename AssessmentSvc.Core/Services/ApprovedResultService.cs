using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.Utils;
using AssessmentSvc.Core.ViewModels;
using AssessmentSvc.Core.ViewModels.Result;
using AssessmentSvc.Core.ViewModels.SessionSetup;
using AssessmentSvc.Core.ViewModels.Student;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.FileStorage;
using Shared.PubSub;
using Shared.Utils;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Shared.Utils.CoreConstants;

namespace AssessmentSvc.Core.Services
{
    public class ApprovedResultService : IApprovedResultService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ApprovedResult, long> _approvedResultRepo;
        private readonly IRepository<Result, long> _resultRepo;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IRepository<SchoolClass, long> _schoolClassRepo;
        private readonly ISessionSetup _sessionService;
        private readonly IResultService _resultService;
        private readonly IResultSummaryService _resultSummaryService;
        private readonly IGradeSetupService _gradeService;
        private readonly IAssessmentSetupService _assessmentSetupService;
        private readonly IPublishService _publishService;
        private readonly IStudentService _studentService;
        private readonly ISchoolService _schoolService;
        private readonly ITeacherService _teacherService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IDocumentService _documentService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IToPDF _toPDF;

        public ApprovedResultService(
            IRepository<ApprovedResult, long> approvedResultRepo,
            IRepository<Result, long> resultRepo,
            IRepository<SchoolClass, long> schoolClassRepo,
            ISessionSetup sessionService,
            IResultService resultService,
            IResultSummaryService resultSummaryService,
            IGradeSetupService gradeService,
            IPublishService publishService,
            IStudentService studentService,
            IAssessmentSetupService assessmentSetupService,
            IRepository<Student, long> studentRepo,
            ISchoolService schoolService,
            ITeacherService teacherService,
            IFileStorageService fileStorageService,
            IDocumentService documentService,
            IToPDF toPDF,
            IHttpClientFactory clientFactory,
        IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _resultService = resultService;
            _sessionService = sessionService;
            _approvedResultRepo = approvedResultRepo;
            _resultRepo = resultRepo;
            _resultSummaryService = resultSummaryService;
            _gradeService = gradeService;
            _assessmentSetupService = assessmentSetupService;
            _publishService = publishService;
            _studentService = studentService;
            _studentRepo = studentRepo;
            _schoolClassRepo = schoolClassRepo;
            _schoolService = schoolService;
            _teacherService = teacherService;
            _fileStorageService = fileStorageService;
            _documentService = documentService;
            _toPDF = toPDF;
            _clientFactory = clientFactory;
        }

        public async Task<ResultModel<string>> SubmitStudentResult(UpdateApprovedStudentResultViewModel vm)
        {
            var result = new ResultModel<string>();

            var sessionResult = await _sessionService.GetCurrentSessionAndTerm();

            if (sessionResult.HasError)
            {
                foreach (string err in sessionResult.ErrorMessages)
                {
                    result.AddError(err);
                }

                return result;
            }

            var currSession = sessionResult.Data;

            //fetch results
            var studResults = await _resultRepo.GetAll()
                .Where(x => x.SessionSetupId == currSession.sessionId && x.TermSequenceNumber == currSession.TermSequence && x.StudentId == vm.StudentId)
                .ToListAsync();

            if (studResults.Count < 1)
            {
                result.AddError("No saved result records for student found.");
                return result;
            }

            //check if result has been sent for approval
            var oldApprovedResult = await _approvedResultRepo.GetAll().Where(x => x.StudentId == vm.StudentId && x.SessionId == currSession.sessionId && x.TermSequence == currSession.TermSequence).FirstOrDefaultAsync();

            //update record if it exist
            if (oldApprovedResult != null)
            {
                oldApprovedResult.ClassTeacherComment = vm.ClassTeacherComment;
                oldApprovedResult.HeadTeacherComment = vm.HeadTeacherComment;
                oldApprovedResult.Results = studResults;
                oldApprovedResult.SchoolClassId = vm.ClassId;
                oldApprovedResult.SessionId = currSession.sessionId;
                oldApprovedResult.TermSequence = currSession.TermSequence;
                oldApprovedResult.StudentId = vm.StudentId;
                oldApprovedResult.ClassTeacherApprovalStatus = vm.ClassTeacherApprovalStatus;
                oldApprovedResult.SchoolAdminApprovalStatus = vm.AdminApprovalStatus;
                oldApprovedResult.HeadTeacherApprovedStatus = vm.HeadTeacherApprovalStatus;
                oldApprovedResult.ClassTeacherId = vm.ClassTeacherId;
                oldApprovedResult.HeadTeacherId = vm.HeadTeacherId;

                await _approvedResultRepo.UpdateAsync(oldApprovedResult);
            }
            else
            {
                var newApprovedResult = new ApprovedResult
                {
                    ClassTeacherComment = vm.ClassTeacherComment,
                    HeadTeacherComment = vm.HeadTeacherComment,
                    Results = studResults,
                    SchoolClassId = vm.ClassId,
                    SessionId = currSession.sessionId,
                    TermSequence = currSession.TermSequence,
                    StudentId = vm.StudentId,
                    ClassTeacherApprovalStatus = vm.ClassTeacherApprovalStatus,
                    SchoolAdminApprovalStatus = vm.AdminApprovalStatus,
                    HeadTeacherApprovedStatus = vm.HeadTeacherApprovalStatus,
                    ClassTeacherId = vm.ClassTeacherId,
                    HeadTeacherId = vm.HeadTeacherId,
                };

                await _approvedResultRepo.InsertAsync(newApprovedResult);
            }

            await _unitOfWork.SaveChangesAsync();

            //ToDO: Optomize
            if (vm.HeadTeacherApprovalStatus == Enumeration.ApprovalStatus.Approved)
            {
                try
                {
                    await _resultSummaryService.CalculateResultSummaries();
                }
                catch (Exception ex)
                {
                    result.Message = $"Result summary error: {ex.Message}";
                }
            }

            result.Data = "Record updated";

            return result;
        }

        public async Task<ResultModel<string>> SubmitClassResultForApproval(UpdateApprovedClassResultViewModel vm)
        {
            var result = new ResultModel<string>();

            var sessionResult = await _sessionService.GetCurrentSchoolSession();

            if (sessionResult.HasError)
            {
                foreach (string err in sessionResult.ErrorMessages)
                {
                    result.AddError(err);
                }

                return result;
            }

            var currSession = sessionResult.Data;

            var currTermSequence = currSession.Terms.FirstOrDefault(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now)?.SequenceNumber;

            if (currTermSequence == null)
            {
                result.AddError("Current term date has expired or its not setup");
            }


            //fetch results
            var classResults = await _resultRepo.GetAll().Include(m => m.ApprovedResult)
                .Where(x => x.SessionSetupId == currSession.Id && x.TermSequenceNumber == currTermSequence && x.SchoolClassId == vm.ClassId)
                .ToListAsync();

            if (classResults.Count < 1)
            {
                result.AddError("No saved result records for student found.");
                return result;
            }

            foreach (var classResult in classResults)
            {
                if (classResult.ApprovedResult != null)
                {
                    classResult.ApprovedResult.ClassTeacherComment = vm.ClassTeacherComment;
                    classResult.ApprovedResult.HeadTeacherComment = vm.HeadTeacherComment;
                    classResult.ApprovedResult.ClassTeacherApprovalStatus = vm.ClassTeacherApprovalStatus;
                    classResult.ApprovedResult.SchoolAdminApprovalStatus = vm.AdminApprovalStatus;
                    classResult.ApprovedResult.HeadTeacherApprovedStatus = vm.HeadTeacherApprovalStatus;
                    classResult.ApprovedResult.ClassTeacherId = vm.ClassTeacherId;
                    classResult.ApprovedResult.HeadTeacherId = vm.HeadTeacherId;
                }
                else
                {
                    classResult.ApprovedResult = new ApprovedResult
                    {
                        ClassTeacherComment = vm.ClassTeacherComment,
                        HeadTeacherComment = vm.HeadTeacherComment,
                        SchoolClassId = vm.ClassId,
                        SessionId = currSession.Id,
                        TermSequence = currTermSequence.Value,
                        StudentId = classResult.StudentId,
                        ClassTeacherApprovalStatus = vm.ClassTeacherApprovalStatus,
                        SchoolAdminApprovalStatus = vm.AdminApprovalStatus,
                        HeadTeacherApprovedStatus = vm.HeadTeacherApprovalStatus,
                        ClassTeacherId = vm.ClassTeacherId,
                        HeadTeacherId = vm.HeadTeacherId,
                    };
                }

                await _resultRepo.UpdateAsync(classResult);
            }

            await _unitOfWork.SaveChangesAsync();

            //ToDO: Optomize
            if (vm.HeadTeacherApprovalStatus == Enumeration.ApprovalStatus.Approved)
            {
                try
                {
                    await _resultSummaryService.CalculateResultSummaries();
                }
                catch (Exception ex)
                {
                    result.Message = $"Result summary error: {ex.Message}";
                }
            }

            result.Data = "Record updated";

            return result;
        }

        public async Task<ResultModel<GetApprovedStudentResultViewModel>> GetStudentResultForApproval(GetStudentResultForApproval vm)
        {
            var result = new ResultModel<GetApprovedStudentResultViewModel>();

            var sessionResult = await _sessionService.GetCurrentSchoolSession();

            if (sessionResult.HasError)
            {
                foreach (string err in sessionResult.ErrorMessages)
                {
                    result.AddError(err);
                }

                return result;
            }

            var currSession = sessionResult.Data;

            var currTermSequence = currSession.Terms.Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).FirstOrDefault()?.SequenceNumber;

            if (currTermSequence == null)
            {
                result.AddError("Current term date has expired or its not setup");
            }

            //check if result has been sent for approval
            var oldApprovedResult = await _approvedResultRepo.GetAll().Where(x => x.StudentId == vm.StudentId && x.SessionId == currSession.Id && x.TermSequence == currTermSequence).FirstOrDefaultAsync();


            if (oldApprovedResult != null)
            {
                var resultsModel = await _resultService.GetStudentResultSheet(vm.ClassId, vm.StudentId);

                if (resultsModel.HasError)
                {
                    foreach (string error in resultsModel.ErrorMessages)
                    {
                        result.AddError(error);
                    }

                    return result;
                }

                var data = new GetApprovedStudentResultViewModel
                {
                    ClassTeacherComment = oldApprovedResult.ClassTeacherComment,
                    HeadTeacherComment = oldApprovedResult.HeadTeacherComment,
                    ClassId = vm.ClassId,
                    SessionId = currSession.Id,
                    TermSequence = currTermSequence.Value,
                    StudentId = vm.StudentId,
                    ClassTeacherApprovalStatus = oldApprovedResult.ClassTeacherApprovalStatus,
                    AdminApprovalStatus = oldApprovedResult.SchoolAdminApprovalStatus,
                    HeadTeacherApprovalStatus = oldApprovedResult.HeadTeacherApprovedStatus,
                    StudentBroadSheet = resultsModel.Data,
                    ClassTeacherId = oldApprovedResult.ClassTeacherId,
                    HeadTeacherId=oldApprovedResult.HeadTeacherId,
                };


                result.Data = data;

                return result;
            }
            else
            {
                result.AddError("No result for approval found");
                return result;
            }
        }

        public async Task<ResultModel<List<ResultBroadSheet>>> GetClassTeacherApprovedClassBroadSheet(long classId)
        {
            //get current term
            //get results for current term
            var result = new ResultModel<List<ResultBroadSheet>>();
            var sessionResult = await _sessionService.GetCurrentSessionAndTerm();

            if (sessionResult.HasError)
            {
                return new ResultModel<List<ResultBroadSheet>>(sessionResult.ErrorMessages);
            }

            var currSession = sessionResult.Data;

            var query = _resultRepo.GetAll()
                 .Where(x => x.SessionSetupId == currSession.sessionId &&
                    x.SchoolClassId == classId &&
                    x.TermSequenceNumber == currSession.TermSequence &&
                    x.ApprovedResult.ClassTeacherApprovalStatus == Enumeration.ApprovalStatus.Approved
                    )
                 .Select(x => new
                 {
                     StudentName = $"{x.Student.FirstName} {x.Student.LastName}",
                     StudentId = x.StudentId,
                     StudentRegNo = x.Student.RegNumber,
                     SubjectName = x.Subject.Name,
                     ScoresJSON = x.ScoresJSON
                 })
                 .ToList();

            if (query.Count < 1)
            {
                result.AddError("No result for this class!");
                return result;
            }

            var queryGroup = query.GroupBy(x => new { x.StudentId, x.StudentName, x.StudentRegNo });
            var data = new List<ResultBroadSheet>();
            foreach (var group in queryGroup)
            {
                var rbroadsheet = new ResultBroadSheet
                {
                    StudentName = group.Key.StudentName,
                    StudentId = group.Key.StudentId,
                    StudentRegNo = group.Key.StudentRegNo
                };

                foreach (var sc in group)
                {

                    var temp = JsonConvert.DeserializeObject<List<Score>>(sc.ScoresJSON, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    var totalscore = temp.Sum(x => x.StudentScore);

                    rbroadsheet.AssessmentAndScores.Add(new SubjectResultBroadSheet { SubjectName = sc.SubjectName, Score = totalscore });
                }

                data.Add(rbroadsheet);

            }

            result.Data = data;


            return result;
        }

        public async Task<ResultModel<StudentReportSheetVM>> GetApprovedResultForStudent(long classId, long? studentId, long? studentUserId, long? curSessionId = null, int? termSequenceNumber = null)
        {
            var result = new ResultModel<StudentReportSheetVM>();
            result.Data = new StudentReportSheetVM();

            //get grade setup for school
            var gradeSetupResult = await _gradeService.GetAllGradeForSchoolSetup();

            if (gradeSetupResult.HasError || gradeSetupResult.Data.Count < 1)
            {
                return new ResultModel<StudentReportSheetVM>("Grade has not setup");
            }

            result.Data.GradeSetup = gradeSetupResult.Data;

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
                return new ResultModel<StudentReportSheetVM>(sessionAndTermResult.ErrorMessages);
            }

            if (!studentId.HasValue && studentUserId.HasValue)
            {
                var student = await _studentRepo.GetAll().FirstOrDefaultAsync(x => x.UserId == studentUserId.Value);

                if (student == null)
                {
                    return new ResultModel<StudentReportSheetVM>("Student not found");
                }

                studentId = student.Id;
            }

            var currSessionAndTerm = sessionAndTermResult.Data;

            result.Data.Session = currSessionAndTerm.SessionName;
            result.Data.Term = currSessionAndTerm.TermName;

            var studentApprovedResults = await _resultRepo.GetAll()
                .Where(x => x.SessionSetupId == currSessionAndTerm.sessionId &&
                    x.SchoolClassId == classId &&
                    x.TermSequenceNumber == currSessionAndTerm.TermSequence &&
                    x.ApprovedResult.ClassTeacherApprovalStatus == Enumeration.ApprovalStatus.Approved &&
                    x.ApprovedResult.HeadTeacherApprovedStatus == Enumeration.ApprovalStatus.Approved)
                .Include(x => x.Subject)
                .ToListAsync();

            if (studentApprovedResults.Count < 1)
            {
                return new ResultModel<StudentReportSheetVM>(errorMessage: "No result found in current term and session");
            }

            var resultsBySubjects = studentApprovedResults.GroupBy(x => x.SubjectId);

            var studResult = new List<SubjectResultBreakdown>();


            foreach (var resultGroup in resultsBySubjects)
            {
                var breakdown = new SubjectResultBreakdown
                {
                    SubjectName = resultGroup.FirstOrDefault(x => x.SubjectId == resultGroup.Key)?.Subject.Name,

                    AssesmentAndScores = resultGroup
                    .Where(x => x.StudentId == studentId)
                    .SelectMany(x => x.Scores)
                    .Select(x => new AssesmentAndScoreViewModel
                    {
                        IsExam = x.IsExam,
                        AssessmentName = x.AssessmentName,
                        StudentScore = x.StudentScore
                    }).ToList()
                };

                //calculate position
                var orderedResults = resultGroup.OrderByDescending(x => x.Scores.Sum(x => x.StudentScore)).ToList();
                var position = orderedResults.IndexOf(orderedResults.Where(x => x.StudentId == studentId).FirstOrDefault());

                breakdown.Position = position + 1;

                //get interpretation
                foreach (var setup in gradeSetupResult.Data)
                {
                    if (breakdown.CummulativeScore >= setup.LowerBound
                        && breakdown.CummulativeScore <= setup.UpperBound)
                    {

                        breakdown.Interpretation = setup.Interpretation;
                        breakdown.Grade = setup.Grade;
                        break;
                    }
                }

                studResult.Add(breakdown);
            }

            result.Data.Breakdowns = studResult;


            var ApprovedResultInfo = await _resultRepo.GetAll()
                .Where(x => x.SessionSetupId == currSessionAndTerm.sessionId &&
                    x.SchoolClassId == classId &&
                    x.TermSequenceNumber == currSessionAndTerm.TermSequence &&
                    x.ApprovedResult.ClassTeacherApprovalStatus == Enumeration.ApprovalStatus.Approved &&
                    x.ApprovedResult.HeadTeacherApprovedStatus == Enumeration.ApprovalStatus.Approved &&
                    x.StudentId == studentId)
                .Select(m => new
                {
                    m.Student.RegNumber,
                    studentName = $"{m.Student.FirstName} {m.Student.LastName}",
                    classs = $"{m.SchoolClass.Name} {m.SchoolClass.ClassArm}",
                    m.Student.DateOfBirth,
                    m.Student.Sex,
                    studentsInClass = m.SchoolClass.Students.Count(),
                    m.ApprovedResult.ClassTeacherComment,
                    m.ApprovedResult.HeadTeacherComment,
                    m.ApprovedResult.ClassTeacherId,
                    m.ApprovedResult.HeadTeacherId,
                })
                .FirstOrDefaultAsync();

            if (ApprovedResultInfo is null)
            {
                return new ResultModel<StudentReportSheetVM>("Result not found");
            }

            result.Data.SubjectOffered = resultsBySubjects.Count();

            result.Data.RegNumber = ApprovedResultInfo.RegNumber;
            result.Data.StudentName = ApprovedResultInfo.studentName;
            result.Data.Class = ApprovedResultInfo.classs;
            result.Data.TotalInClass = ApprovedResultInfo.studentsInClass;
            result.Data.ClassTeacherComment = ApprovedResultInfo.ClassTeacherComment;
            result.Data.HeadTeacherComment = ApprovedResultInfo.HeadTeacherComment;
            result.Data.ClassTeacherId = ApprovedResultInfo.ClassTeacherId;
            result.Data.HeadTeacherId = ApprovedResultInfo.HeadTeacherId;
            result.Data.Sex = ApprovedResultInfo.Sex;
            result.Data.Age = DateTime.Now.Year - ApprovedResultInfo.DateOfBirth.Year;

            return result;
        }

        public async Task<ResultModel<List<StudentReportSheetVM>>> GetApprovedResultForMultipleStudents(long classId, long[] studentIds, long? curSessionId = null, int? termSequenceNumber = null)
        {

            var result = new ResultModel<List<StudentReportSheetVM>>
            {
                Data = new List<StudentReportSheetVM>()
            };

            //get grade setup for school
            var gradeSetupResult = await _gradeService.GetAllGradeForSchoolSetup();

            if (gradeSetupResult.HasError || gradeSetupResult.Data.Count < 1)
            {
                return new ResultModel<List<StudentReportSheetVM>>("Grade has not been setup");
            }


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
                return new ResultModel<List<StudentReportSheetVM>>(sessionAndTermResult.ErrorMessages);
            }

            var currSessionAndTerm = sessionAndTermResult.Data;

            var studentsApprovedResults = await _resultRepo.GetAll()
                .Where(x => x.SessionSetupId == currSessionAndTerm.sessionId &&
                    x.SchoolClassId == classId &&
                    x.TermSequenceNumber == currSessionAndTerm.TermSequence &&
                    x.ApprovedResult.ClassTeacherApprovalStatus == Enumeration.ApprovalStatus.Approved &&
                    x.ApprovedResult.HeadTeacherApprovedStatus == Enumeration.ApprovalStatus.Approved)
                .Include(x => x.Subject)
                .Select(m => new
                {
                    Results = m,
                    m.Student.RegNumber,
                    m.Student.Sex,
                    studentName = $"{m.Student.FirstName} {m.Student.LastName}",
                    classs = $"{m.SchoolClass.Name} {m.SchoolClass.ClassArm}",
                    studentsInClass = m.SchoolClass.Students.Count(),
                    m.ApprovedResult.ClassTeacherComment,
                    m.ApprovedResult.HeadTeacherComment,
                    m.StudentId,
                    m.Student.DateOfBirth,
                    m.ApprovedResult.ClassTeacherId,
                    m.ApprovedResult.HeadTeacherId,
                })
                .ToListAsync();

            if (studentsApprovedResults.Count < 1)
            {
                return new ResultModel<List<StudentReportSheetVM>>(errorMessage: "No result found in current term and session");
            }
            var uniqueStudentsResults = studentsApprovedResults.GroupBy(x => x.Results.StudentId).ToList();


            if (uniqueStudentsResults.Select(x => x.Key).Count() != studentIds.Length)
            {
                for (int i = 0; i < studentIds.Length; i++)
                {
                    if (!uniqueStudentsResults.Select(x => x.Key).Contains(studentIds[i]))
                    {
                        result.AddError($"No result for student with id {studentIds[i]}");
                    }
                }
            }


            var resultsBySubjects = studentsApprovedResults.GroupBy(x => x.Results.SubjectId);

            foreach (var studentApprovedResults in uniqueStudentsResults)
            {
                if (studentIds.Contains(studentApprovedResults.Key))
                {
                    var studResult = new List<SubjectResultBreakdown>();

                    foreach (var resultGroup in resultsBySubjects)
                    {
                        var breakdown = new SubjectResultBreakdown
                        {
                            SubjectName = resultGroup.FirstOrDefault(x => x.Results.SubjectId == resultGroup.Key)?.Results.Subject.Name,

                            AssesmentAndScores = resultGroup
                            .Where(x => x.Results.StudentId == studentApprovedResults.Key)
                            .SelectMany(x => x.Results.Scores)
                            .Select(x => new AssesmentAndScoreViewModel
                            {
                                IsExam = x.IsExam,
                                AssessmentName = x.AssessmentName,
                                StudentScore = x.StudentScore
                            }).ToList()
                        };

                        //calculate position
                        var orderedResults = resultGroup.OrderByDescending(x => x.Results.Scores.Sum(x => x.StudentScore)).ToList();
                        var position = orderedResults.IndexOf(orderedResults.FirstOrDefault(x => x.Results.StudentId == studentApprovedResults.Key));

                        breakdown.Position = position + 1;

                        //get interpretation
                        foreach (var setup in gradeSetupResult.Data)
                        {
                            if (breakdown.CummulativeScore >= setup.LowerBound
                                && breakdown.CummulativeScore <= setup.UpperBound)
                            {

                                breakdown.Interpretation = setup.Interpretation;
                                breakdown.Grade = setup.Grade;
                                break;
                            }
                        }

                        studResult.Add(breakdown);
                    }

                    var t = studentsApprovedResults.FirstOrDefault(n => n.StudentId == studentApprovedResults.Key);
                    result.Data.Add(new StudentReportSheetVM
                    {
                        StudentId = studentApprovedResults.Key,
                        Breakdowns = studResult,
                        SubjectOffered = resultsBySubjects.Count(),
                        RegNumber = t.RegNumber,
                        StudentName = t.studentName,
                        Sex = t.Sex,
                        Age = DateTime.Now.Year - t.DateOfBirth.Year,
                        Class = t.classs,
                        ClassTeacherComment = t.ClassTeacherComment,
                        HeadTeacherComment = t.HeadTeacherComment,
                        ClassTeacherId = t.ClassTeacherId,
                        HeadTeacherId = t.HeadTeacherId,
                        GradeSetup = gradeSetupResult.Data,
                        TotalInClass = t.studentsInClass,
                        Session = currSessionAndTerm.SessionName,
                        Term = currSessionAndTerm.TermName
                    });

                }
            }

            return result;
        }

        public async Task<ResultModel<List<StudentVM>>> GetStudentsWithApprovedResult(long classId, long? curSessionId = null, int? termSequenceNumber = null)
        {

            var result = new ResultModel<List<StudentVM>>();

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
                return new ResultModel<List<StudentVM>>(sessionAndTermResult.ErrorMessages);
            }

            var currSessionAndTerm = sessionAndTermResult.Data;

            var studentWithApprovedResults = await _resultRepo.GetAll()
                .Where(x => x.SessionSetupId == currSessionAndTerm.sessionId &&
                    x.SchoolClassId == classId &&
                    x.TermSequenceNumber == currSessionAndTerm.TermSequence &&
                    x.ApprovedResult.ClassTeacherApprovalStatus == Enumeration.ApprovalStatus.Approved &&
                    x.ApprovedResult.HeadTeacherApprovedStatus == Enumeration.ApprovalStatus.Approved
                ).Select(m => new StudentVM()
                {
                    Id = m.Student.Id,
                    Name = $"{m.Student.LastName} {m.Student.FirstName}",
                    RegNumber = m.Student.RegNumber,
                })
                .ToListAsync();

            if (studentWithApprovedResults.Count < 1)
            {
                return new ResultModel<List<StudentVM>>(errorMessage: "No Approved result found in current term and session");
            }

            List<StudentVM> eachStudent = studentWithApprovedResults.GroupBy(x => x.Id).Select(m => m.First()).ToList();
            result.Data = eachStudent;

            return result;
        }

        private async Task<Dictionary<long, string>> GetStudentsResultPDFS(List<StudentReportSheetVM> vm, long classId, long curSessionId, int termSequenceNumber, long tenantId)
        {
            var headTeacherIdParams = vm.Select(m => m.HeadTeacherId).Distinct().Select(m => $"UserIds={m}");

            //http://localhost:58101/api/v1/Staff/GetStaffNamesAndSignaturesByUserIds?UserIds=9&UserIds=6&GetBytes=false
            string url = $"api/v1/Staff/GetStaffNamesAndSignaturesByUserIds?{string.Join('&', headTeacherIdParams)}&GetBytes=false";

            HttpClient client = _clientFactory.CreateClient("localclient");
            client.DefaultRequestHeaders.Add("tenantId", tenantId.ToString()); 
            var headTeacherTask = client.GetAsync<ApiResponse<List<StaffNameAndSignatureVM>>>(url);

            var assessmentSetup = await _assessmentSetupService.GetAllAssessmentSetup();

            var totalScore = assessmentSetup.Data.Sum(m => m.MaxScore);
            var totalExamScore = assessmentSetup.Data.Where(m => m.Name.ToLower().Contains("xam")).Sum(m => m.MaxScore);
            var totalCAScore = assessmentSetup.Data.Where(m => !m.Name.ToLower().Contains("xam")).Sum(m => m.MaxScore);

            var school = await _schoolService.GetSchool(tenantId) ?? new School() ;
            var schoolLogoMemeType = school.Logo?.EndsWith("png") == true ? "data:image/png;" : "data:image/jpeg;";
            school.Logo = $"{schoolLogoMemeType}base64, {_documentService.TryGetUploadedFile(school.Logo)}";

            var behaviours = await _resultService.GetBehaviouralResults(new GetBehaviourResultQueryVm() { ClassId = classId, SessionId = curSessionId, TermSequence = termSequenceNumber });

            var classTeachers = await _teacherService.GetTeachersByUserIdsAsync(vm.Select(m => m.ClassTeacherId).Distinct().ToList());
            classTeachers.ForEach(m => {
                var classTeacherMemeType = m.Signature?.EndsWith("png") == true ? "data:image/png;" : "data:image/jpeg;";
                m.Signature = $"{classTeacherMemeType}base64, {_documentService.TryGetUploadedFile(m.Signature)}";
                });
            
            var templatePath = _fileStorageService.MapStorage(CoreConstants.ResultPdfTemplatePath);

            var studentFilePaths = new Dictionary<long, string> ();

            var headTeachers = (await headTeacherTask)?.Payload;

            headTeachers = headTeachers == null ? new List<StaffNameAndSignatureVM>() : headTeachers;

            headTeachers.ForEach(m => {
                var headTeacherMemeType = m.Signature?.EndsWith("png") == true ? "data:image/png;" : "data:image/jpeg;";
                m.Signature = $"{headTeacherMemeType}base64, {_documentService.TryGetUploadedFile(m.Signature)}";
            });

            client.Dispose();

            foreach (var result in vm)
            {
                var tableObjects = new List<TableObject<object>>();


                var objList = new List<object>();
                var totalExamScoreObtained = 0d;
                var totalCAScoreObtained = 0d;

                foreach (var bd in result.Breakdowns)
                {
                    dynamic obj = new ExpandoObject();
                    var dictionary = obj as IDictionary<string, object>;
                    //var dictionary = new Dictionary<string, object>();

                    dictionary.Add("Subject", bd.SubjectName);
                    foreach (var assessment in bd.AssesmentAndScores)
                    {
                        if (assessment.AssessmentName.ToLower().Contains("xam"))
                        {
                            totalExamScoreObtained += assessment.StudentScore;
                        }
                        else
                        {
                            totalCAScoreObtained += assessment.StudentScore;
                        }

                        dictionary.Add(assessment.AssessmentName, assessment.StudentScore.ToString());
                    }
                    dictionary.Add("Cumulative", bd.CummulativeScore.ToString());
                    dictionary.Add("Grade", bd.Grade.ToString());
                    dictionary.Add("Interpretation", bd.Interpretation.ToString());

                    objList.Add(dictionary);
                }

                var scoresTable = new TableObject<object>()
                {
                    TableConfig = new TableAttributeConfig
                    {
                        TableAttributes = new { @class = "tContent" },
                    },
                    TemplatePropertyName = "ScoresTable",
                    TableData = objList,
            };

                var gradeSetupTable = new TableObject<object>()
                {
                    TableConfig = new TableAttributeConfig
                    {
                        TableAttributes = new { @class = "tContent" },
                    },
                    TemplatePropertyName = "GradeSetupTable",
                    TableData = result.GradeSetup.Select(m=> new { Grade_Scale = $"{m.UpperBound}-{m.LowerBound}", Grade = m.Grade, Interpretation = m.Interpretation})
                };

                tableObjects = new List<TableObject<object>> { scoresTable, gradeSetupTable };

                var studBehaviours = behaviours.FirstOrDefault(m => m.Key == result.StudentId);
                var tableArrays = new List<KeyValuePair<string, IEnumerable<TableObject<object>>>>();

                var BehaviourTables = new List<TableObject<object>>();

                if (!studBehaviours.Equals(default(KeyValuePair<long, GetBehaviourResultVM>)))
                {
                    foreach (var item in studBehaviours.Value.ResultTypeAndValues)
                    {
                        var tableData = new List<object>();

                        foreach (var behavior in item.Value)
                        {
                            dynamic behahiourObject = new ExpandoObject();
                            var behaviourDictionary = behahiourObject as IDictionary<string, object>;

                            behaviourDictionary.Add(item.Key, behavior.BehaviourName);
                            behaviourDictionary.Add("Grade", behavior.Grade);

                            tableData.Add((object)behahiourObject);
                        }

                        BehaviourTables.Add(new TableObject<object>()
                        {
                            TableConfig = new TableAttributeConfig
                            {
                                TableAttributes = new { @class = "tContent" },
                            },
                            TableData = tableData
                        }
                        );
                    }

                }
                tableArrays.Add(new KeyValuePair<string, IEnumerable<TableObject<object>>>( "Behaviours", BehaviourTables));
                var classTeacher = classTeachers.FirstOrDefault(m => m.UserId == result.ClassTeacherId) ?? new Teacher();
                var headTeacher = headTeachers.FirstOrDefault(m => m.UserId == result.HeadTeacherId) ?? new StaffNameAndSignatureVM();

                var mainData = new
                {
                    Class = result.Class,
                    Session = result.Session,
                    Term = result.Term,
                    StudentName = result.StudentName,
                    StudentRegNum = result.RegNumber,
                    StudentAge = result.Age,
                    StudentSex = result.Sex,
                    Total_Exam = totalExamScore * result.SubjectOffered,
                    Total_CA = totalCAScore * result.SubjectOffered,
                    Total_Score = totalScore * result.SubjectOffered,
                    Total_Score_Obtained = Math.Round(totalCAScoreObtained + totalExamScoreObtained, 2),
                    Total_CA_Score = Math.Round(totalCAScoreObtained, 2),
                    Total_Exam_Score = Math.Round(totalExamScoreObtained, 2),
                    SchoolName = school.Name,
                    SchoolCity = school.City,
                    SchoolState = school.State,
                    SchoolPhone = school.PhoneNumber,
                    SchoolEmail = school.Email,
                    SchoolWebsite = school.WebsiteAddress,
                    ImgPath = school.Logo,
                    ClassTeacherName = $"{classTeacher.LastName} {classTeacher.FirstName}",
                    ClassTeacherComment = result.ClassTeacherComment,
                    ClassTeacherSignature = classTeacher.Signature,
                    HeadTeacherComment = result.HeadTeacherComment,
                    HeadTeacherSignature = headTeacher.Signature,
                    HeadTeacherName = $"{headTeacher.LastName} {headTeacher.FirstName}"
                };

                studentFilePaths.Add(result.StudentId, _toPDF.ResultToPDF(mainData, tableObjects, tableArrays, templatePath, false));
            }

            return studentFilePaths;
        }

        public async Task<ResultModel<string>> MailResult(MailResultVM vm)
        {
            var result = new ResultModel<string>();

            var sessionAndTermResult = new ResultModel<CurrentSessionAndTermVM>();

            if (vm.curSessionId != null && vm.termSequenceNumber != null)
            {
                sessionAndTermResult = await _sessionService.GetSessionAndTerm(vm.curSessionId.Value, vm.termSequenceNumber.Value);
            }
            else
            {
                sessionAndTermResult = await _sessionService.GetCurrentSessionAndTerm();
            }

            if (sessionAndTermResult.HasError)
            {
                return new ResultModel<string>(sessionAndTermResult.ErrorMessages);
            }

            var currSessionAndTerm = sessionAndTermResult.Data;

            var mailInfos = await _studentService.GetParentsMailInfo(vm.StudentIds);
            if (mailInfos.HasError)
            {
                return new ResultModel<string>(mailInfos.ErrorMessages);
            }

            var resultData = await GetApprovedResultForMultipleStudents(vm.classId, vm.StudentIds, currSessionAndTerm.sessionId, currSessionAndTerm.TermSequence);

            if (resultData.HasError)
            {
                return new ResultModel<string>(resultData.ErrorMessages);
            }
            //generate pdf
            var pdfPaths = await GetStudentsResultPDFS(resultData.Data, vm.classId, currSessionAndTerm.sessionId, currSessionAndTerm.TermSequence, currSessionAndTerm.TenantId);


            await _publishService.PublishMessage(Topics.Notification, BusMessageTypes.NOTIFICATION, new CreateNotificationModel
            {
                Emails = mailInfos.Data.Select(m => new CreateEmailModel(
                   EmailTemplateType.StudentResult,
                   new Dictionary<string, string>{
                            { "link", $"{vm.ResultPageURL}?studId={m.StudentId}&classId={vm.classId}&sessionId={currSessionAndTerm.sessionId}&termSequenceNumber={currSessionAndTerm.TermSequence}" },
                            { "ParentName", m.ParentName },
                            {"Studentname", m.StudentName},
                   },
                   new UserVM() { FullName = m.ParentName, Email = m.Email },
                   new List<string> { pdfPaths[m.StudentId] }
                )).ToList()
            });

            result.Data = "Successful";
            return result;
        }

        public async Task<ResultModel<List<ClassResultApprovalVM>>> GetClassesResultApproval(long? curSessionId = null, int? termSequenceNumber = null)
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
                return new ResultModel<List<ClassResultApprovalVM>>(sessionAndTermResult.ErrorMessages);
            }

            var classes = await _schoolClassRepo.GetAll()
                .Include(m => m.Students)
                .ToListAsync();

            if (classes.Count < 1)
            {
                return new ResultModel<List<ClassResultApprovalVM>>("No Class was found.");
            }

            var schoolApprovedResultSummariesResult = await _resultSummaryService.GetResultSummaries(sessionAndTermResult.Data.sessionId, sessionAndTermResult.Data.TermSequence);

            if (schoolApprovedResultSummariesResult.HasError || schoolApprovedResultSummariesResult.Data is null)
            {
                return new ResultModel<List<ClassResultApprovalVM>>(errorMessage: "No result summary found in current term and session");
            }
            var schoolApprovedResultSummaries = schoolApprovedResultSummariesResult.Data;

            var rtnData = new List<ClassResultApprovalVM> ();

            foreach (var clas in classes)
            {
                var allStudentApproved = false;
                var studentResult = new ResultSummary();
                // do i make sure that all the individual student's result in all subjects are approved?
                foreach (var classStudent in clas.Students)
                {
                    studentResult = schoolApprovedResultSummaries.FirstOrDefault(m => m.StudentId == classStudent.Id);
                    
                    if (studentResult != null && studentResult.ResultApproved == true)
                    {
                        allStudentApproved = true;
                    }
                    else
                    {
                        allStudentApproved = false;
                        break;
                    }
                }

                rtnData.Add(new ClassResultApprovalVM()
                {
                    ClassName = $"{clas.Name} {clas.ClassArm}",
                    isApproved = allStudentApproved,
                    DateCreated = studentResult==null ? "" : studentResult.CreationTime.ToShortDateString()
                } ) ;
            }

            return new ResultModel<List<ClassResultApprovalVM>>(rtnData);

        }

    }
}
