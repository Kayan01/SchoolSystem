using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Tenancy
{
    public interface ITenantModelType
    {
        public long TenantId { get; set; }
    }
}
