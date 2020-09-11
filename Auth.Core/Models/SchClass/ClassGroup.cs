using Auth.Core.Models.JoinTables;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models
{
    /// <summary>
    /// Setup class group such as jss1A , jss1B etc
    /// </summary>
    public class ClassGroup : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// join table between class and groups
        /// </summary>
        public List<Class2Group> ClassJoinGroup { get; set; }
    }
}
