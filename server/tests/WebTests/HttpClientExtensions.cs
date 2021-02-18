using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebTests
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> Get(this HttpClient client, string uri)
        {
            return Send(client, new HttpRequestMessage(HttpMethod.Get, uri));
        }

        public static Task<HttpResponseMessage> Post(this HttpClient client, string uri, object data = null)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, uri);
            return Send(client, message, data);
        }

        public static Task<HttpResponseMessage> Put(this HttpClient client, string uri, object data = null)
        {
            var message = new HttpRequestMessage(HttpMethod.Put, uri);
            return Send(client, message, data);
        }

        public static Task<HttpResponseMessage> Delete(this HttpClient client, string uri)
        {
            return Send(client, new HttpRequestMessage(HttpMethod.Delete, uri));
        }

        private static Task<HttpResponseMessage> Send(
            HttpClient client, HttpRequestMessage message, object data = null)
        {
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
    }
}
