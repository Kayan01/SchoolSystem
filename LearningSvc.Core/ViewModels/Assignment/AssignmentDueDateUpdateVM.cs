using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LearningSvc.Core.ViewModels.Assignment
{
    public class AssignmentDueDateUpdateVM
    {
        [Required]
        public long AssignmentId { get; set; }
        [Required]
        public DateTime NewDate { get; set; }
    }
}
