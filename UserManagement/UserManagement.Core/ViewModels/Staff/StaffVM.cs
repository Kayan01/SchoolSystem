using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Core.ViewModels.Staff
{
    public class StaffVM
    {
        public string  FirstName { get; set; }
        public string LastName { get; set; }
        public long Id { get;  internal set; }

        public static implicit operator StaffVM(Models.Staff model)
        {
            return model == null ? null : new StaffVM
            {
                Id = model.Id,
                FirstName = model.FirstName
            };
        }
    }
}
