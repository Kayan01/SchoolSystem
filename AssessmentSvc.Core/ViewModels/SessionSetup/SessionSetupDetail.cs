using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.SessionSetup
{
    public class SessionSetupDetail
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsCurrent { get; set; }
        public List<TermVM> Terms { get; set; }
    }
}
