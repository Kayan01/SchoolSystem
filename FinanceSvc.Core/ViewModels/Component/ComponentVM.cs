using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Component
{
    public class ComponentVM
    {
        public long Id { get; set; }
        public long AccountId { get; set; }

        public string Name { get; set; }
        public string Terms { get; set; }
        public int SequenceNumber { get; set; }

        public bool IsActive { get; set; }

        public string Account { get; set; }
    }
}
