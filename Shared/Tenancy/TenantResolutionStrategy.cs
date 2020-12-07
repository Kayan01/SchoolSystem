using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Utils.CoreConstants;

namespace Shared.Tenancy
{
    public interface ITenantResolutionStrategy
    {
        long GetTenantIdentifier();
    }

    public class TenantHeaderResolutionStrategy : ITenantResolutionStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private StringValues tenantIds = new StringValues();

        public TenantHeaderResolutionStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get the tenant identifier
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public long GetTenantIdentifier()
        {
            if (_httpContextAccessor.HttpContext == null)
                return 0;

            var tenantClaim = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimsKey.TenantId);

            if (tenantClaim == null)
            {
                throw new Exception("No tenant Id provided");
            }

            var result = long.TryParse(tenantClaim?.Value, out long tenantId);

            if (result == false)
            {
                throw new Exception("Tenant Id provided is not in proper format");
            }
            else if (tenantId < 1)
            {
                throw new Exception("No tenant Id provided");
            }

            return tenantId;
        }
    }
}
