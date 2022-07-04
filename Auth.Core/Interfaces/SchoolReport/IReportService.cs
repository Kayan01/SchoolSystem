using Auth.Core.ViewModels.ReportDetail;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces.SchoolReport
{
    public interface IReportService
    {
        //Total Number Of Active Students
        ///Total Number Of Classes
        ///Toatal Number of Student in Each Class
        ///Total Number Of Teachers In School
        ///Total Number of Non Academic Staff in School
        ///Class Teacher Of Each Clsss
        ///Get All Parents in School Tied to a Students
        ///All Schools in Aplication
        ///Total active Students Count in Each School
        ///All teachers in School
        ///All Non Academic Staff In Each School
        ///All School And their current Subscription Dates.
        ///

        Task<ResultModel<SchoolReportVM>> generateSchoolReport(long SchoolId);
        Task<ResultModel<ClassReportVM>> getClassReport(long classId);
        Task<ResultModel<IEnumerable<AdminLevelSchoolsReport>>> getSchoolDetailsForAdmin();
    }
}
