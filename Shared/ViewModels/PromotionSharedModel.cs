using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class PromotionSharedModel
    {
        public long TenantId { get; set; }
        public long SessionId { get; set; }
        public string SessionName { get; set; }
        public int MaxRepeats { get; set; }
        public int CutOff { get; set; }
        public bool IsAutomaticPromotion { get; set; }

        public List<StudentPromotionInfo> StudentPromotionInfoList { get; set; }
    }

    public class StudentPromotionInfo
    {
        public long StudentId { get; set; }
        public bool PassedCutoff { get; set; }
        public double Average { get; set; }
    }
}
