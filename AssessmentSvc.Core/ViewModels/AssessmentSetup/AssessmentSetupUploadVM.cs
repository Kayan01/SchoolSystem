using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.AssessmentSetup
{
    public class AssessmentSetupUploadVM
    {
        public int SequenceNumber { get; set; }
        public string Name { get; set; }
        public int MaxScore { get; set; }
    }
}
