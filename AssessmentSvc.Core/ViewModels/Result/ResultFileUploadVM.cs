using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class ResultFileUploadVM
    {
        public long SchoolClassId { get; set; }
        public long SubjectId { get; set; }
        [Required(ErrorMessage ="Please upload excel file")]
        public IFormFile ExcelFile { get; set; }
    }
}
