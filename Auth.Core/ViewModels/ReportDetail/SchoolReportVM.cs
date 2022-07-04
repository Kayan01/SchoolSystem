using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.ReportDetail
{
    public class SchoolReportVM
    {
        public long? SchoolId { get; set; }
        public string SchoolName { get; set; }
        public int NumberOfStudents { get; set; }
        public int NumberOfClasses { get; set; }
        public long NumberOfTeachers { get; set; }
        public long NumberOfNonAcademicStaffs { get; set; }


    }


    public class ClassReportVM
    {
        public long ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassTeacher { get; set; }
        public int NumberOFStudentsInClass { get; set; }
         
    }

    public class AdminLevelSchoolsReport
    {
        public string SchoolName { get; set; }
        public long NumberOfStudents { get; set; }
        public long NumberOfAcademicStaffs { get; set; }
        public long NumberOfNonAcademicStaffs { get; set; }
        public long NumberOfClassInSchool { get; set; }

    }

    public class SchoolSubscriptionReport
    {
        public string SchoolName { get; set; }
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }

    }
}
