using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Core.ViewModels
{
    public class PasswordResetModel
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class PasswordResetQueryModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
