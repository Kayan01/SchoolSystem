using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AssessmentSvc.Core.ViewModels
{
    public class AddIncidenceVm
    {
        public DateTime OccurenceDate { get; set; }

        public string Description { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public int TermSequence { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long StudentId { get; set; }

    }
    public class GetIncidenceVm
    {
        public DateTime OccurenceDate { get; set; }

        public string Description { get; set; }

    }

    public class GetIncidenceQueryVm
    {

        [Required]
        public long SessionId { get; set; }
        [Required]
        public int TermSequence { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long StudentId { get; set; }
    }


}
