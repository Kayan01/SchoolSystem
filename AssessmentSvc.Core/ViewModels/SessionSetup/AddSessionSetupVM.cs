using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssessmentSvc.Core.ViewModels.SessionSetup
{
    public class AddSessionSetupVM
    {
        public List<TermVM> Terms { get; set; }
        public string SessionName { get; set; }
        public bool IsCurrent { get; set; }
    }
}