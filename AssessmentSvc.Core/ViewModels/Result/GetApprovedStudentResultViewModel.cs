using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public  class GetApprovedStudentResultViewModel : UpdateApprovedStudentResultViewModel
    {        
        public IndividualBroadSheet StudentBroadSheet { get; set; }
    }
}
