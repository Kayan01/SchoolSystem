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

        public List<StudentPromotion> StudentPromotionList { get; set; }
    }

    public class StudentPromotion
    {
        public long StudentId { get; set; }
        public bool PassedCutoff { get; set; }
        public double Average { get; set; }
    }
}
