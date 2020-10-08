using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class StaffSharedModel : PersonSharedModel
    {
        public long TenantId { get; set; }
        public long Id { get; set; }
        public StaffType StaffType { get; set; }
    }
}
