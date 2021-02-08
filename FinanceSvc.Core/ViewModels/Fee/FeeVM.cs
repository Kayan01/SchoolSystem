using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Fee
{
    public class FeeVM
    {
        public long Id { get; set; }
        public long SchoolClassId { get; set; }
        public long FeeGroupId { get; set; }

        public string Name { get; set; }
        public string Terms { get; set; }
        public int SequenceNumber { get; set; }

        public bool IsActive { get; set; }

        public string Class { get; set; }
        public string FeeGroup { get; set; }
    }
}
