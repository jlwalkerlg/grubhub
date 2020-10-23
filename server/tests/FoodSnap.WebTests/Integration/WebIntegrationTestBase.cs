using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FoodSnap.Domain.Users;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Services.Tokenization;
using Xunit;

namespace FoodSnap.WebTests.Integration
{
    [Collection(nameof(WebAppIntegrationTestFixture))]
    public class WebIntegrationTestBase : IAsyncLifetime
    {
        protected readonly WebAppIntegrationTestFixture fixture;

        private HttpClient client;
        private string authToken;

        public WebIntegrationTestBase(WebAppIntegrationTestFixture fixture)
        {
            this.fixture = fixture;
        }

        public async Task InitializeAsync()
        {
            await fixture.ResetDatabase();

            client = fixture.Factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }

        public async Task Login(User user)
        {
            await Login(user.Id.Value);
        }

        public async Task Login(Guid id)
        {
            await fixture.ExecuteService<ITokenizer>(tokenizer =>
            {
                authToken = tokenizer.Encode(id.ToString());
                return Task.CompletedTask;
            });
        }

        public void Logout()
        {
            authToken = null;
        }

        private Task<HttpResponseMessage> Send(HttpRequestMessage message, object data = null)
        {
            if (authToken != null)
            {
                message.Headers.Add("Cookie", $"auth_token={authToken}");
            }
            if (data != null)
            {
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            return client.SendAsync(message);
        }

        protected Task<HttpResponseMessage> Get(string uri)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, uri);
            return Send(message);
        }

        protected async Task<T> Get<T>(string uri)
        {
            var response = await Get(uri);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStreamAsync();
            var envelope = await JsonSerializer.DeserializeAsync<DataEnvelope<T>>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
            return envelope.Data;
        }

        protected Task<HttpResponseMessage> Post(string uri, object data = null)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, uri);
            return Send(message, data);
        }

        protected Task<HttpResponseMessage> Put(string uri, object data = null)
        {
            var message = new HttpRequestMessage(HttpMethod.Put, uri);
            return Send(message, data);
        }

        protected Task<HttpResponseMessage> Delete(string uri)
        {
            var message = new HttpRequestMessage(HttpMethod.Delete, uri);
            return Send(message);
        }
    }

}
