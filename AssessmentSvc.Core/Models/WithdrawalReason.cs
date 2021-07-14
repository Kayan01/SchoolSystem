using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models
{
    public class WithdrawalReason : AuditedEntity<long>
    {
        public long PromotionSetupId { get; set; }
        public string Reason { get; set; }

        public PromotionSetup PromotionSetup { get; set; }
    }
}
