using AssessmentSvc.Core.Enumeration;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models
{
    public class PromotionSetup : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public PromotionMethod PromotionMethod { get; set; }
        public PromotionType PromotionType { get; set; }
        public int PromotionScore { get; set; }
        public int MaxRepeat { get; set; }

        public List<WithdrawalReason> WithdrawalReasons { get; set; }
    }
}
