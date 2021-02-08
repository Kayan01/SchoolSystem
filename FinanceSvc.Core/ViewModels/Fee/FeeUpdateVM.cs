using FinanceSvc.Core.ViewModels.FeeComponent;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Fee
{
    public class FeeUpdateVM
    {
        public long SchoolClassId { get; set; }
        public long FeeGroupId { get; set; }

        public string Name { get; set; }
        public int[] Terms { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public string terms
        {
            get { return string.Join(',', Terms); }
        }
        public int SequenceNumber { get; set; }

        public bool IsActive { get; set; }

    }
}
