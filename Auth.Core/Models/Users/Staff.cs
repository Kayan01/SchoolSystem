using Shared.Enums;

namespace Auth.Core.Models
{
    public class Staff : Person
    {
        public StaffType StaffType { get; set; }

    }
}