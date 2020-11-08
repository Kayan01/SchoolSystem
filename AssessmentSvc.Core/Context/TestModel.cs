using Shared.Entities.Auditing;
using Shared.Tenancy;

namespace AssessmentSvc.Core.Context
{
    public class TestModel : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string Name { get; set; }
    }
}