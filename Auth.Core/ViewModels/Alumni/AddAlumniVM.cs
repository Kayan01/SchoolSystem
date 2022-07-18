using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Alumni
{
    public class AddAlumniVM
    {
       public long StudId { get; set; }
        public string SessionName { get; set; }
        public string TermName { get; set; }
        public string Reason { get; set; }
    }
}
