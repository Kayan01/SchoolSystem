using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.RoleModels
{
    public class RoleRequestModel : PagedRequestModel
    {
        public long RoleId { get; set; }
    }
}
