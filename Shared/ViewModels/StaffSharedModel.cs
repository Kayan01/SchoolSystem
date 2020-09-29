using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class StaffSharedModel : PersonSharedModel
    {
        public StaffType StaffType { get; set; }
    }
}
