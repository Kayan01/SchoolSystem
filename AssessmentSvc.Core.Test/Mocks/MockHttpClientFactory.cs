using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Test.Mocks
{
    class MockHttpClientFactory
	{
		public Mock<IHttpClientFactory> Mock = new Mock<IHttpClientFactory>();

		public MockHttpClientFactory()
		{
			HttpResponseMessage httpResponse = new HttpResponseMessage();
			httpResponse.StatusCode = System.Net.HttpStatusCode.OK;
			httpResponse.Content = new StringContent("", Encoding.UTF8, "application/json");

			string url = "GetStaffNamesAndSignaturesByUserIds";
			Mock<HttpMessageHandler> MockMessageHandler = new Mock<HttpMessageHandler>();
			MockMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync",
					ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().Contains(url)),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(httpResponse);

			HttpClient httpClient = new HttpClient(MockMessageHandler.Object);
			httpClient.BaseAddress = new Uri("http://13.88.13.159:58100/schtrack-auth/");
			Mock = new Mock<IHttpClientFactory>();
			Mock.Setup(x => x.CreateClient("localclient"))
					.Returns(httpClient);
		}

	}
}
