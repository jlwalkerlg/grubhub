using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Web;

namespace WebTests
{
    public class HttpTestClient
    {
        private readonly HttpClient client;
        private string userId;

        public HttpTestClient(WebApplicationFactory<Startup> factory)
        {
            client = factory.CreateClient(
                new WebApplicationFactoryClientOptions()
                {
                    AllowAutoRedirect = false,
                    HandleCookies = true,
                });

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Task<HttpResponseMessage> Get(string uri)
        {
            return Send(new HttpRequestMessage(HttpMethod.Get, uri));
        }

        public Task<HttpResponseMessage> Post(string uri, object data = null)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, uri);
            return Send(message, data);
        }

        public Task<HttpResponseMessage> Put(string uri, object data = null)
        {
            var message = new HttpRequestMessage(HttpMethod.Put, uri);
            return Send(message, data);
        }

        public Task<HttpResponseMessage> Delete(string uri)
        {
            return Send(new HttpRequestMessage(HttpMethod.Delete, uri));
        }

        private Task<HttpResponseMessage> Send(HttpRequestMessage message, object data = null)
        {
            if (userId != null)
            {
                message.Headers.Add("user-id", userId);
            }

            if (data != null)
            {
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return client.SendAsync(message);
        }

        public HttpTestClient Authenticate(Guid userId)
        {
            return Authenticate(userId.ToString());
        }

        public HttpTestClient Authenticate(string userId)
        {
            this.userId = userId;
            return this;
        }
    }
}
