using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class StudentSharedModel : PersonSharedModel
    {
        public long Id { get; set; }
        public long? ClassId { get; set; }
    }
}
