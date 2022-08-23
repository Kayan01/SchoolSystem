using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels
{
    public class ExportPayloadVM
    {
        public string FileName { get; set; }
        public string Base64String { get; set; }
    }

    public class StaffTypeVM
    {
        public StaffType Staff { get; set; }
    }
}
