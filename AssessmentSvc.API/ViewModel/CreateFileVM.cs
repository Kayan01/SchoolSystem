using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentSvc.API.ViewModel
{
    public class CreateFileVM
    {
        public string FileName { get; set; }

        [Required]
        public IFormFile Document { get; set; }

    }
}
