using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace WebTests
{
    [Trait("Category", "ActionTest")]
    public class ActionTestBase : IClassFixture<ActionTestWebApplicationFactory>
    {
        protected readonly ActionTestWebApplicationFactory factory;

        public ActionTestBase(ActionTestWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        protected HttpClient GetClient()
        {
            var client = factory.CreateClient(
                new WebApplicationFactoryClientOptions()
                {
                    AllowAutoRedirect = false,
                    HandleCookies = true,
                });

            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        protected HttpClient GetAuthenticatedClient()
        {
            return GetAuthenticatedClient(Guid.NewGuid());
        }

        protected HttpClient GetAuthenticatedClient(Guid userId)
        {
            var client = GetClient();

            client.DefaultRequestHeaders.Add("X-USER-ID", userId.ToString());

            return client;
        }
    }
}
