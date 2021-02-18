using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace WebTests
{
    public static class WebApplicationFactoryExtensions
    {
        public static HttpClient GetClient<TEntryPoint>(
            this WebApplicationFactory<TEntryPoint> factory)
            where TEntryPoint : class
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

        public static HttpClient GetAuthenticatedClient<TEntryPoint>(
            this WebApplicationFactory<TEntryPoint> factory) where TEntryPoint : class
        {
            return factory.GetAuthenticatedClient(Guid.NewGuid());
        }

        public static HttpClient GetAuthenticatedClient<TEntryPoint>(
            this WebApplicationFactory<TEntryPoint> factory, Guid userId) where TEntryPoint : class
        {
            var client = factory.GetClient();

            client.DefaultRequestHeaders.Add("X-USER-ID", userId.ToString());

            return client;
        }

        public static async Task<TRequest> Send<TEntryPoint, TRequest>(
            this WebApplicationFactory<TEntryPoint> factory, MediatR.IRequest<TRequest> request)
            where TEntryPoint : class
        {
            using var scope = factory.Services.CreateScope();
            var sender = scope.ServiceProvider.GetRequiredService<MediatR.ISender>();

            return await sender.Send(request);
        }
    }
}
