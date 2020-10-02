using Auth.Core.Models.Users;
using Auth.Core.ViewModels.SchoolClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Staff
{
    public class TeacherVM : StaffVM
    {
        public long? ClassId { get; set; }
        public ClassVM Class { get; set; }

        public static implicit operator TeacherVM (TeachingStaff model)
        {
            return model == null ? null : new TeacherVM
            {
                Id = model.Id,
                Email = model.Staff?.User?.Email,
                FirstName = model.Staff?.User?.FirstName,
                LastName = model.Staff?.User?.LastName,
                PhoneNumber = model.Staff?.User?.PhoneNumber,
                ClassId = model.ClassId,
                Class = model.Class,
            };
        }
    }

    public class AddTeacherVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public long? ClassId { get; set; }
    }

    public class UpdateTeacherVM
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public long? ClassId { get; set; }
    }
}
