using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class ParentSharedModel : PersonSharedModel
    {
        public long Id { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public string SecondaryEmail { get; set; }
        public string HomeAddress { get; set; }
        public string OfficeAddress { get; set; }
    }
}
