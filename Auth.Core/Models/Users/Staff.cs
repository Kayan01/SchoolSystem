using Auth.Core.Models.Users;
using Shared.Enums;

namespace Auth.Core.Models
{
    public class Staff : Person
    {
        public StaffType StaffType { get; set; }
        public TeachingStaff TeachingStaff { get; set; }
    }
}