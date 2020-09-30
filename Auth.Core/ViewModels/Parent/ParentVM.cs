using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Parent
{
    public class ParentVM
    {
        public static explicit operator ParentVM(Models.Users.Parent v)
        {
            throw new NotImplementedException();
        }
    }
}
