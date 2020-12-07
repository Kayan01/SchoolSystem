using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models
{
    public abstract class Person : AuditedEntity<long>
    {
        public long UserId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
