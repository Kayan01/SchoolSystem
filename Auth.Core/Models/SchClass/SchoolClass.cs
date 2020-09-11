using Auth.Core.Models.JoinTables;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models
{
    /// <summary>
    /// Defines a class in a school
    /// </summary>
    public class SchoolClass : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string Name { get; set; }
        public long SchoolSectionId { get; set; }
        public SchoolSection SchoolSection { get; set; }

        /// <summary>
        /// join table between class and groups
        /// </summary>
        public List<Class2Group> ClassJoinGroup { get; set; } = new List<Class2Group>();

        public ICollection<Student> Students { get; set; } = new List<Student>();

    }
}
