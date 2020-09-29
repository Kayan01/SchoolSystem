using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class TeacherSharedModel : StaffSharedModel
    {
        public long ClassId { get; set; }
    }
}
