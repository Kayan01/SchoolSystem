using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class ResultFileUploadVM
    {
        public long SchoolClassId { get; set; }
        public long SubjectId { get; set; }
        public IFormFile ExcelFile { get; set; }
    }
}
