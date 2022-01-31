using AssessmentSvc.Core.ViewModels.GradeSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class StudentReportSheetVM
    {
        public string StudentName { get; set; }
        public int Age { get; set; }
        public string RegNumber { get; set; }
        public string Sex { get; set; }
        public string Class { get; set; }
        public string Session { get; set; }
        public string Term { get; set; }
        public int TotalSubject { get; set; }
        public int SubjectOffered { get; set; }
        public int TotalInClass { get; set; }
        public int Position { get; set; }
        public long ClassTeacherId { get; set; }
        public long HeadTeacherId { get; set; }
        public string ClassTeacherComment { get; set; }
        public string HeadTeacherComment { get; set; }
        public string ClassTeacherSignature { get; set; }
        public string HeadTeacherSignature { get; set; }
        public List<SubjectResultBreakdown> Breakdowns { get; set; } = new List<SubjectResultBreakdown>();
        public List<GradeSetupListVM> GradeSetup { get; set; }
        public long StudentId { get; internal set; }
        public long? NoOfTimesAbsent { get; set; }
        public long? NoOfTimesPresent { get; set; }
        public long? TotalNoOfSchoolDays { get; set; }

        

    }
}
