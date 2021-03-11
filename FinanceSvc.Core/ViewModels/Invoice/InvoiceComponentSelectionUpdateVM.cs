using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Invoice
{
    public class InvoiceComponentSelectionUpdateVM
    {
        public long InvoiceId { get; set; }

        public List<ComponentSelection> ComponentSelections { get; set; }
    }

    public class ComponentSelection
    {
        public long ComponentId { get; set; }
        public bool IsSelected { get; set; }
    }
}
