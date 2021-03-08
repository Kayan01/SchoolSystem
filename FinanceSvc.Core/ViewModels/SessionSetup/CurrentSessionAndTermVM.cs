using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.SessionSetup
{
    public class CurrentSessionAndTermVM
    {
        public long sessionId { get; set; }
        public string SessionName { get; set; }
        public int TermSequence { get; set; }
        public string TermName { get; set; }
    }
}
