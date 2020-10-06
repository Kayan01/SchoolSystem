using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Tenancy
{
    public class TenantInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            StringValues tenantIds = new StringValues();
            try
            {
                var tenantInfo = httpContext.RequestServices.GetRequiredService<TenantInfo>();
                httpContext.Request.Headers.TryGetValue("tenantId", out tenantIds);

                long.TryParse(tenantIds, out long tenantId);

                tenantInfo.TenantId = tenantId;
                
                
            }
            catch(Exception ex)
            {

            }
            finally
            {
                await _next(httpContext);
            }
        }
    }
}
