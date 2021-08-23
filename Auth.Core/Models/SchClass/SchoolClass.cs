using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Auth.Core.Models
{
    /// <summary>
    /// Defines a class in a school
    /// </summary>
    public class SchoolClass : FullAuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string Name { get; set; }
        public string ClassArm { get; set; }
        public bool IsActive { get; set; }
        public long SchoolSectionId { get; set; }
        public int Sequence { get; set; }
        public bool IsTerminalClass { get; set; }
        [NotMapped]
        public string FullName { get { return $"{Name} {ClassArm}"; } }
        public SchoolSection SchoolSection { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}