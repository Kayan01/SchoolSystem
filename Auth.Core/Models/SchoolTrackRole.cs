using Shared.Entities.Common;
using Shared.Tenancy;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Core.Models
{
    //This Model was added to manage Roles per Tenant, since roles are unique per tenant
    public class SchoolTrackRole : Entity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public string RoleName => $"{Name}|{TenantId}";
    }
}
