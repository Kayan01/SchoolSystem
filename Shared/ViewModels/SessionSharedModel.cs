using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class SessionSharedModel
    {
        public long Id { get; set; }
        public long TenantId { get; set; }

        public string SessionName { get; set; }
        public bool IsCurrent { get; set; }

        public string TermsJSON { get; set; } //will contain List of Term object as JSON string

    }
}
