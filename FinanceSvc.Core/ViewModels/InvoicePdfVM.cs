using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels
{
   public class InvoicePdfVM
    {
       public KeyValuePair<dynamic, IEnumerable<dynamic>> KeyValuePair { get; set; }
       public string ParentEmail { get; set; }

        public string StudentEmail { get; set; }
        public string StudentName { get; set; }
    }


}
