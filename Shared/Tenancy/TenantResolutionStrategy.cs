﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Shared.Exceptions;
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
                //check for tenancy in header if its not present in clams
                var tenantId = CheckTenancyInHeader();

                return tenantId;

            }
            else
            {
                var result = long.TryParse(tenantClaim?.Value, out long tenantId);


                if (!result)
                {
                    throw new TenantNotFoundException("Tenant Id provided is not in proper format");
                }
                else if (tenantId < 1)
                {
                    throw new TenantNotFoundException("No tenant Id provided");
                }

                return tenantId;
            }
            
        }

        private long CheckTenancyInHeader()
        {
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("tenantId", out StringValues tenantIds);
            var firstTenant = tenantIds.FirstOrDefault();


            if (tenantIds.Count < 1 || firstTenant == null)
            {
                throw new TenantNotFoundException("No tenant Id provided");
            }

            long tId;
            try
            {
                tId = long.Parse(firstTenant);
            }
            catch (Exception)
            {

                throw new TenantNotFoundException("Tenant Id provided is not in proper format");
            }

            return tId;
        }
    }
}
