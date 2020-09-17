using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FoodSnap.Infrastructure.Persistence.EF;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FoodSnap.WebTests.Functional
{
    [Collection("WebApp")]
    public abstract class FunctionalTestBase : IDisposable
    {
        private readonly IServiceScope scope;
        private readonly HttpClient client;
        protected readonly AppDbContext appDbContext;

        public FunctionalTestBase(WebAppFactory factory)
        {
            scope = factory.Services.CreateScope();

            client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            appDbContext = GetService<AppDbContext>();
            appDbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            appDbContext.Database.EnsureDeleted();
            scope.Dispose();
        }

        protected T GetService<T>()
        {
            return scope.ServiceProvider.GetService<T>();
        }

        protected Task<HttpResponseMessage> GetJson(string uri)
        {
            return client.GetAsync(uri);
        }

        protected Task<HttpResponseMessage> PostJson(string uri, object data = null)
        {
            HttpContent content = null;
            if (data != null)
            {
                var json = JsonSerializer.Serialize(data);
                content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            return client.PostAsync(uri, content);
        }
    }
}
