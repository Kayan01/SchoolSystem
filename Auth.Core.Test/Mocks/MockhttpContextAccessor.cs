using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using static Shared.Utils.CoreConstants;

namespace Auth.Core.Test.Mocks
{
    public class MockhttpContextAccessorclass
    {
        public Mock<IHttpContextAccessor> Mock { get; }

        public MockhttpContextAccessorclass()
        {
            Mock = new Mock<IHttpContextAccessor>();

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimsKey.TenantId, "4")
                }, "mock"));
            Mock.Setup(_ => _.HttpContext).Returns(httpContext);
        }
    }
}
