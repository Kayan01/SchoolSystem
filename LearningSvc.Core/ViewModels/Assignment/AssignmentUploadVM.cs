using Microsoft.AspNetCore.Http;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.Assignment
{
    public class AssignmentUploadVM
    {
        public long Id { get; set; }

        public string Title { get; set; }
        public long SubjectId { get; set; }
        public long ClassId { get; set; }
        public long TeacherId { get; set; }

        public DateTime DueDate { get; set; }
        public int TotalScore { get; set; }
        public IFormFile Document { get; set; }

    }
}
