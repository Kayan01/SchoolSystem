using Microsoft.AspNetCore.Http;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LearningSvc.Core.ViewModels.Assignment
{
    public class AssignmentUploadVM
    {
        public string Title { get; set; }
        public string Comment { get; set; }
        public long ClassSubjectId { get; set; }

        public DateTime DueDate { get; set; }
        public int TotalScore { get; set; }
        [Required]
        public IFormFile Document { get; set; }

    }
}
