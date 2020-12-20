using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.ViewModels.AssessmentSetup;
using AssessmentSvc.Core.ViewModels.Result;
using AssessmentSvc.Core.ViewModels.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace AssessmentSvc.Core.Services
{
    public class ResultService : IResultService
    {
        private readonly IRepository<Result, long> _resultRepo;
        private readonly IAssessmentSetupService _assessmentService;
        private readonly IStudentService _studentServive;
        public readonly IGradeSetupService _gradeService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISessionSetup _sessionService;

        public ResultService(IUnitOfWork unitOfWork,
            IRepository<Result, long> resultRepo,
            IAssessmentSetupService assessmentService,
            IStudentService studentServive,
            ISessionSetup sessionService,
            IGradeSetupService gradeService)
        {
            _unitOfWork = unitOfWork;
            _resultRepo = resultRepo;
            _assessmentService = assessmentService;
            _studentServive = studentServive;
            _gradeService = gradeService;
            _sessionService = sessionService;
        }

        public async Task<ResultModel<ResultUploadFormData>> FetchResultUploadFormData(long SchoolClassId)
        {
            var result = new ResultModel<ResultUploadFormData>();

            var assessments = await _assessmentService.GetAllAssessmentSetup();
            if (assessments.Data?.Any() != true)
            {
                result.AddError("Assessments has not been setup!");
                return result;
            }

            var students = await _studentServive.GetStudentsByClass(SchoolClassId);
            if (students.Data?.Any() != true)
            {
                result.AddError("No student available in this class!");
                return result;
            }

            result.Data = new ResultUploadFormData()
            {
                Assessments = assessments.Data,
                Students = students.Data,
            };

            return result;
        }

        public async Task<ResultModel<byte[]>> GenerateResultUploadExcel(long SchoolClassId)
        {
            var result = new ResultModel<byte[]>();

            var rtn = await FetchResultUploadFormData(SchoolClassId);
            if (rtn.HasError)
            {
                foreach (var error in rtn.ErrorMessages)
                {
                    result.AddError(error);
                }
                return result;
            }

            var data = rtn.Data;

            var workbook = new XSSFWorkbook();
            var myFont = (XSSFFont)workbook.CreateFont();
            myFont.FontHeightInPoints = 11;
            myFont.FontName = "Arial";


            // Defining a border
            var borderedCellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            borderedCellStyle.SetFont(myFont);
            borderedCellStyle.BorderLeft = BorderStyle.Medium;
            borderedCellStyle.BorderTop = BorderStyle.Medium;
            borderedCellStyle.BorderRight = BorderStyle.Medium;
            borderedCellStyle.BorderBottom = BorderStyle.Medium;
            borderedCellStyle.VerticalAlignment = VerticalAlignment.Center;

            ISheet Sheet = workbook.CreateSheet("Report");
            //Create The Headers of the excel
            IRow HeaderRow = Sheet.CreateRow(0);

            //Create The Actual Cells
            CreateCell(HeaderRow, 0, "S/N", borderedCellStyle);
            CreateCell(HeaderRow, 1, "UUID", borderedCellStyle);
            CreateCell(HeaderRow, 2, "Student Name", borderedCellStyle);
            CreateCell(HeaderRow, 3, "Reg Number", borderedCellStyle);

            for (int i = 4; i < data.Assessments.Count + 4; i++)
            {
                var assessment = data.Assessments[i - 4];

                CreateCell(HeaderRow, i, $"{assessment.Name} (over {assessment.MaxScore})", borderedCellStyle);
            }

            // This is where the Data row starts from
            int RowIndex = 1;

            //Iteration through the collection
            foreach (var student in data.Students)
            {
                //Creating the CurrentDataRow
                IRow CurrentRow = Sheet.CreateRow(RowIndex);
                CreateCell(CurrentRow, 0, RowIndex.ToString(), borderedCellStyle);
                CreateCell(CurrentRow, 1, student.Id.ToString(), borderedCellStyle);
                CreateCell(CurrentRow, 2, student.Name, borderedCellStyle);
                CreateCell(CurrentRow, 3, student.RegNumber, borderedCellStyle);

                RowIndex++;
            }

            // Auto sized all the affected columns
            int lastColumNum = Sheet.GetRow(0).LastCellNum;
            for (int i = 0; i <= lastColumNum; i++)
            {
                Sheet.AutoSizeColumn(i);
                GC.Collect();
            }

            // output the XLSX file
            using (var ms = new MemoryStream())
            {
                workbook.Write(ms, leaveOpen: true);
                ms.Seek(0, SeekOrigin.Begin);
                result.Data = ms.ToArray();
            }

            return result;
        }

        public async Task<ResultModel<string>> ProcessResult(ResultUploadVM model)
        {
            var result = new ResultModel<string>();

            var currentSessionResult = await _sessionService.GetCurrentSessionAndTerm();

            if (currentSessionResult.HasError)
            {
                foreach (var error in currentSessionResult.ErrorMessages)
                {
                    result.AddError(error);
                }
                return result;
            }

            foreach (var studentresult in model.StudentResults)
            {
                var resultObject = new Result()
                {
                    SchoolClassId = model.ClassId,
                    SessionSetupId = currentSessionResult.Data.sessionId,
                    StudentId = studentresult.StudentId,
                    SubjectId = model.SubjectId,
                    TermSequenceNumber = currentSessionResult.Data.TermSequence,
                    Scores = studentresult.AssessmentAndScores.Select(m => new Score()
                    {
                        AssessmentName = m.AssessmentName,
                        StudentScore = m.Score,
                    }).ToList(),
                };

                _resultRepo.Insert(resultObject);
            }

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved";
            return result;
        }

        public async Task<ResultModel<string>> ProcessResultFromExcel(ResultFileUploadVM vM)
        {
            var result = new ResultModel<string>();

            IWorkbook workbook = ReadWorkbook(vM.ExcelFile);
            var sheet = workbook.GetSheetAt(0);

            var assessments = await _assessmentService.GetAllAssessmentSetup();
            if (assessments.Data?.Any() != true)
            {
                result.AddError("Assessment setting has not been setup!");
                return result;
            }

            var students = await _studentServive.GetStudentsByClass(vM.SchoolClassId);
            if (students.Data?.Any() != true)
            {
                result.AddError("No student available in this class!");
                return result;
            }

            var studentdata = students.Data;

            if (!validateWorksheet(sheet, assessments.Data, studentdata.Count))
            {
                result.AddError("The Excel does not pass validation.");
                return result;
            }

            var resultVM = new ResultUploadVM()
            {
                ClassId = vM.SchoolClassId,
                SubjectId = vM.SubjectId,
                StudentResults = new List<StudentResult>()
            };

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) 
                {
                    result.AddError("Encountered invalid row");
                    return result;
                }

                if (row.Cells.All(d => d.CellType == CellType.Blank))
                {
                    result.AddError("Encountered invalid row");
                    return result;
                }

                int id;

                if (!int.TryParse((row.GetCell(row.FirstCellNum + 1)).ToString(), out id))
                {
                    result.AddError("Encountered invalid Student Id");
                    return result;
                }

                var student = studentdata.Where(m => m.Id == id).FirstOrDefault();

                if (student == null)
                {
                    result.AddError("Encountered invalid Student Id");
                    return result;
                }

                if (student.Name.ToLowerInvariant() != row.GetCell(row.FirstCellNum + 2).ToString().ToLowerInvariant())
                {
                    result.AddError("Encountered invalid data. Student Id does not match name.");
                    return result;
                }

                List<AssessmentAndScore> scores = new List<AssessmentAndScore>();

                for (int j = (row.FirstCellNum + 4); j < row.LastCellNum; j++)
                {
                    int cellScore ;

                    if (!int.TryParse((row.GetCell(j)).ToString(), out cellScore))
                    {
                        cellScore = 0;
                    }

                    var assessment = assessments.Data[j - 4];
                    scores.Add(new AssessmentAndScore()
                    {
                        AssessmentName = assessment.Name,
                        Score = cellScore
                    });
                }

                resultVM.StudentResults.Add(new StudentResult() 
                { 
                    StudentId = student.Id,
                    AssessmentAndScores = scores,
                });

                studentdata.Remove(student);
            }

            return await ProcessResult(resultVM);
        }

        private bool validateWorksheet(ISheet sheet, List<AssessmentSetupVM> assessments, int studentCount)
        {
            var headerValues = new List<string>() { "S/N", "UUID", "Student Name", "Reg Number" };

            foreach (var item in assessments)
            {
                headerValues.Add($"{item.Name} (over {item.MaxScore})");
            }

            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null)
            {
                return false;
            }

            int cellCount = headerRow.LastCellNum;
            if (cellCount != headerValues.Count)
            {
                return false;
            }

            for (int i = 0; i < cellCount; i++)
            {
                ICell cell = headerRow.GetCell(i);

                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) return false;

                if (cell.ToString().ToLowerInvariant() != headerValues[i].ToLowerInvariant())
                {
                    return false;
                }
            }

            var rowcount = sheet.LastRowNum - sheet.FirstRowNum; // count rows except header row.
            if (rowcount != studentCount)
            {
                return false;
            }

            return true;
        }

        private void CreateCell(IRow CurrentRow, int CellIndex, string Value, XSSFCellStyle Style)
        {
            ICell Cell = CurrentRow.CreateCell(CellIndex);
            Cell.SetCellValue(Value);
            Cell.CellStyle = Style;
        }

        // Attemps to read workbook as XLSX, then XLS, then fails.
        private IWorkbook ReadWorkbook(IFormFile file)
        {
            IWorkbook book;

            try
            {
                // Try to read workbook as XLSX:
                try
                {
                    book = new XSSFWorkbook(file.OpenReadStream());
                }
                catch
                {
                    book = null;
                }

                // If reading fails, try to read workbook as XLS:
                if (book == null)
                {
                    book = new HSSFWorkbook(file.OpenReadStream());
                }

                return book;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ResultModel<List<ResultBroadSheet>>> GetClassBroadSheet(long classId)
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

            var query = _resultRepo.GetAll()
                 .Where(x => x.SessionSetupId == currSession.Id && x.SchoolClassId == classId)
                 .Select(x => new
                 {
                     StudentName = $"{x.Student.FirstName} {x.Student.LastName}",
                     StudentRegNo = x.Student.RegNumber,
                     SubjectName = x.Subject.Name,
                     ScoresJSON = x.ScoresJSON
                 })
                 .ToList();

            if (query.Count < 1)
            {
                result.AddError("No rsult for this class!");
                return result;
            }

            var queryGroup = query.GroupBy(x => x.StudentName);
            var data = new List<ResultBroadSheet>();
            foreach (var group in queryGroup)
            {
                var rbroadsheet = new ResultBroadSheet
                {
                    StudentName = group.Key,
                    StudentRegNo = group.FirstOrDefault().StudentRegNo
                };

                foreach (var sc  in group)
                {

                    var temp = JsonConvert.DeserializeObject<List<Score>>(sc.ScoresJSON, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    var totalscore = temp.Sum(x => x.StudentScore);

                    rbroadsheet.AssessmentAndScores.Add(new SubjectResultBroadSheet { SubjectName = sc.SubjectName, Score = totalscore }) ;
                }

                data.Add(rbroadsheet);
               
            }

            result.Data = data;


            return result;
        }

        public async Task<ResultModel<IndividualBroadSheet>> GetStudentResultSheet(long classId, long studentId)
        {

            var result = new ResultModel<IndividualBroadSheet>();

            //get grade setup for school
            var gradeSetupResult = await _gradeService.GetAllGradeForSchoolSetup();

            if (gradeSetupResult.HasError || gradeSetupResult.Data.Count < 1)
            {
                result.AddError("Grade has not setup");
                return result; 
            }


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

            var currTermSequence = currSession.Terms.Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).FirstOrDefault().SequenceNumber;


            var classResults = await _resultRepo.GetAll()
                .Where(x => x.SessionSetupId == currSession.Id && x.SchoolClassId == classId && x.TermSequenceNumber == currTermSequence)
                .Include(x=> x.Subject)
                .ToListAsync();

            if (classResults.Count < 1)
            {
                result.AddError("No result found in current term and session");
                return result;
            }

            var resultsBySubjects = classResults.GroupBy(x => x.SubjectId);

            var studResult = new IndividualBroadSheet();


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
                        && breakdown.CummulativeScore <= setup.UpperBound) {
                        
                        breakdown.Interpretation = setup.Interpretation;
                        breakdown.Grade = setup.Grade;
                        break;
                    }
                }

                studResult.Breakdowns.Add(breakdown);
            }

            result.Data = studResult;
            return result;
        }
    }
}
