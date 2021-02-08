using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Component
{
    public class ComponentPostVM
    {
        public long AccountId { get; set; }

        public string Name { get; set; }
        [Required]
        public int[] Terms { get; set; }
        public int SequenceNumber { get; set; }

        public bool IsActive { get; set; }
    }
}
