using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels
{
    public class StaffNameAndSignatureVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Signature { get; set; }
        public long UserId { get; set; }
    }
}
