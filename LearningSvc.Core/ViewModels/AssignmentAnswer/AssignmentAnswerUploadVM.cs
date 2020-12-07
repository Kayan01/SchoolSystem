using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LearningSvc.Core.ViewModels.AssignmentAnswer
{
    public class AssignmentAnswerUploadVM
    {
        public long Id { get; set; }

        public long AssignmentId { get; set; }

        [Required]
        public IFormFile Document { get; set; }
    }
}
