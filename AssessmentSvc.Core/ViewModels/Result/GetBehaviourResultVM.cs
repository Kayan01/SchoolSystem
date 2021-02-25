using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class GetBehaviourResultVM
    {
        public Dictionary<string, List<BehaviourValuesAndGrade>> ResultTypeAndValues { get; set; }
    }


    public class GetBehaviourResultQueryVm
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