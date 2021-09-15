using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Promotion
{
    public class ClassPoolVM
    {
        public long Id { get; set; }
        public string StudentName { get; set; }
        public string RegNumber { get; set; }
        public string Level { get; set; }
        public string PreviousClass { get; set; }
        public double Average { get; set; }
        public string WithdrawalReason { get; set; }

        public long ToClass { get; set; }
        public ClassPoolVM_PromotionStatus Status { get; set; }
        public string ReInstateReason { get; set; }


        public enum ClassPoolVM_PromotionStatus
        {
            Promoted,
            Repeated,
            Withdrawn,
            Graduated,
            ReInstated
        }
    }

}
