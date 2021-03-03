using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class AddBehaviourResultVM
    {
        public long SessionId { get; set; }
        public int TermSequence { get; set; }
        public long ClassId { get; set; }
        public long StudentId { get; set; }
        public Dictionary<string, List<BehaviourValuesAndGrade>> ResultTypeAndValues { get; set; }
    } 
    public class BehaviourValuesAndGrade
    {
        public string BehaviourName { get; set; }
        public string Grade { get; set; }
    }

}
