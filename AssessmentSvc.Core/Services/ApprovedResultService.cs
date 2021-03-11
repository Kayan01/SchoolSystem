using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.ViewModels.Result;
using AssessmentSvc.Core.ViewModels.SessionSetup;
using AssessmentSvc.Core.ViewModels.Student;
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
using static Shared.Utils.CoreConstants;

namespace AssessmentSvc.Core.Services
{
    public class ApprovedResultService : IApprovedResultService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ApprovedResult, long> _approvedResultRepo;
        private readonly IRepository<Result, long> _resultRepo;
        private readonly ISessionSetup _sessionService;
        private readonly IResultService _resultService;
        private readonly IGradeSetupService _gradeService;
        private readonly IPublishService _publishService;
        private readonly IStudentService _studentService;

        public ApprovedResultService(
            IRepository<ApprovedResult, long> approvedResultRepo,
            IRepository<Result, long> resultRepo,
            ISessionSetup sessionService, 
            IResultService resultService,
            IGradeSetupService gradeService,
            IPublishService publishService, 
            IStudentService studentService,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _resultService = resultService;
            _sessionService = sessionService;
            _approvedResultRepo = approvedResultRepo;
            _resultRepo = resultRepo;
            _gradeService = gradeService;
            _publishService = publishService;
            _studentService = studentService;
        }
        public async Task<ResultModel<string>> SubmitStudentResult(UpdateApprovedStudentResultViewModel vm)
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

            var currTermSequence = currSession.Terms.Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).FirstOrDefault()?.SequenceNumber;

            if (currTermSequence == null)
            {
                result.AddError("Current term date has expired or its not setup");
            }


            //fetch results
            var studResults = await _resultRepo.GetAll()
                .Where(x => x.SessionSetupId == currSession.Id && x.TermSequenceNumber == currTermSequence && x.StudentId == vm.StudentId)
                .ToListAsync();

            if (studResults.Count < 1)
            {
                result.AddError("No saved result records for student found.");
                return result;
            }

            //check if result has been sent for approval
            var oldApprovedResult = await _approvedResultRepo.GetAll().Where(x => x.StudentId == vm.StudentId && x.SessionId == currSession.Id && x.TermSequence == currTermSequence).FirstOrDefaultAsync();

            //update record if it exist
            if (oldApprovedResult != null)
            {
                oldApprovedResult.ClassTeacherComment = vm.ClassTeacherComment;
                oldApprovedResult.HeadTeacherComment = vm.HeadTeacherComment;
                oldApprovedResult.Results = studResults;
                oldApprovedResult.SchoolClassId = vm.ClassId;
                oldApprovedResult.SessionId = currSession.Id;
                oldApprovedResult.TermSequence = currTermSequence.Value;
                oldApprovedResult.StudentId = vm.StudentId;
                oldApprovedResult.ClassTeacherApprovalStatus = vm.ClassTeacherApprovalStatus;
                oldApprovedResult.SchoolAdminApprovalStatus = vm.AdminApprovalStatus;
                oldApprovedResult.HeadTeacherApprovedStatus = vm.HeadTeacherApprovalStatus;

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
                    SessionId = currSession.Id,
                    TermSequence = currTermSequence.Value,
                    StudentId = vm.StudentId,
                    ClassTeacherApprovalStatus = vm.ClassTeacherApprovalStatus,
                    SchoolAdminApprovalStatus = vm.AdminApprovalStatus,
                     HeadTeacherApprovedStatus = vm.HeadTeacherApprovalStatus
                };

                await _approvedResultRepo.InsertAsync(newApprovedResult);
            }

            await _unitOfWork.SaveChangesAsync();

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
            var classResults = await _resultRepo.GetAll().Include(m=>m.ApprovedResult)
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
                        HeadTeacherApprovedStatus = vm.HeadTeacherApprovalStatus
                    };
                }

                await _resultRepo.UpdateAsync(classResult);
            }
            
            await _unitOfWork.SaveChangesAsync();

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

            var query = _resultRepo.GetAll()
                 .Where(x => x.SessionSetupId == currSession.Id &&
                    x.SchoolClassId == classId &&
                    x.TermSequenceNumber == currTermSequence &&
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

        public async Task<ResultModel<StudentReportSheetVM>> GetApprovedResultForStudent(long classId, long studentId, long? curSessionId = null, int? termSequenceNumber = null)
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

            var currSessionAndTerm = sessionAndTermResult.Data;

            result.Data.Session = currSessionAndTerm.SessionName;
            result.Data.Term = currSessionAndTerm.TermName;

            var studentApprovedResults = await _resultRepo.GetAll()
                .Where(x => x.SessionSetupId == currSessionAndTerm.sessionId &&
                    x.SchoolClassId == classId &&
                    x.TermSequenceNumber == currSessionAndTerm.TermSequence &&
                    x.ApprovedResult.ClassTeacherApprovalStatus == Enumeration.ApprovalStatus.Approved &&
                    x.ApprovedResult.HeadTeacherApprovedStatus == Enumeration.ApprovalStatus.Approved &&
                    x.StudentId == studentId)
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
                .Select( m=> new { 
                    m.Student.RegNumber,
                    studentName = $"{m.Student.FirstName} {m.Student.LastName}",
                    classs = $"{m.SchoolClass.Name} {m.SchoolClass.ClassArm}",
                    studentsInClass = m.SchoolClass.Students.Count(),
                    m.ApprovedResult.ClassTeacherComment,
                    m.ApprovedResult.HeadTeacherComment,
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
                ).Select(m=> new StudentVM()
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

            List<StudentVM> eachStudent = studentWithApprovedResults.GroupBy(x => x.Id).Select(m=>m.First()).ToList();
            result.Data = eachStudent;

            return result;
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


            await _publishService.PublishMessage(Topics.Notification, BusMessageTypes.NOTIFICATION, new CreateNotificationModel
            {
                Emails = mailInfos.Data.Select(m => new CreateEmailModel(
                   EmailTemplateType.StudentResult,
                   new Dictionary<string, string>{
                            { "link", $"{vm.ResultPageURL}?studId={m.StudentId}&classId={vm.classId}&sessionId={currSessionAndTerm.sessionId}&termSequenceNumber={currSessionAndTerm.TermSequence}" },
                            { "ParentName", m.ParentName },
                            {"Studentname", m.StudentName}
                   },
                   new UserVM() { FullName = m.ParentName, Email = m.Email })).ToList()
            });

            result.Data = "Successful";
            return result;
        }

    }
}
