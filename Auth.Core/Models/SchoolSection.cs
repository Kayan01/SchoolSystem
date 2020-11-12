using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Auth.Core.Models
{
    /// <summary>
    /// Defines the school sections such as Secondary, primary , nursery etc
    /// </summary>
    public class SchoolSection : FullAuditedEntity<long>, ITenantModelType
    {
        [ForeignKey(nameof(School))]
        public long TenantId { get; set; }
        public string Name { get; set; }

        public School School { get; set; }
        public List<SchoolClass> Classes { get; set; }
    }
}
