using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class MailResultVM
    {
        public long[] StudentIds { get; set; }
        public string ResultPageURL { get; set; }
        public long classId { get; set; }
        public long? curSessionId { get; set; } = null;
        public int? termSequenceNumber { get; set; } = null;
    }
}
