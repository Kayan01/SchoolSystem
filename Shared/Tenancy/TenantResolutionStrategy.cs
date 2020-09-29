using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            try
            {
                _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("tenantId", out tenantIds);

                var firstTenant = tenantIds.FirstOrDefault();


                if (tenantIds.Count < 1 || firstTenant == null)
                {
                    throw new Exception("No tenant Id provided");
                }

                long tId;
                try
                {
                    tId = long.Parse(firstTenant);
                }
                catch (Exception)
                {

                    throw new Exception("Tenant Id provided is not in proper format");
                }

                return tId;
            }
            catch(Exception e)
            {
                return 0;
            }
            /*_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("tenantId", out tenantIds);

            var firstTenant = tenantIds.FirstOrDefault();


            if (tenantIds.Count < 1 || firstTenant == null)
            {
                throw new Exception("No tenant Id provided");
            }

            long tId;
            try
            {
               tId =   long.Parse(firstTenant);
            }
            catch (Exception)
            {

                throw new Exception("Tenant Id provided is not in proper format");
            }
            
            return tId;*/
        }
    }
}
