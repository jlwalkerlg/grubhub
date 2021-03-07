using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Web.Domain.Users;
using User = WebTests.TestData.User;

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
            this WebApplicationFactory<TEntryPoint> factory,
            UserRole role = UserRole.RestaurantManager) where TEntryPoint : class
        {
            return factory.GetAuthenticatedClient(Guid.NewGuid(), role);
        }

        public static HttpClient GetAuthenticatedClient<TEntryPoint>(
            this WebApplicationFactory<TEntryPoint> factory,
            Guid userId,
            UserRole role = UserRole.RestaurantManager) where TEntryPoint : class
        {
            var client = factory.GetClient();

            client.DefaultRequestHeaders.Add("X-USER-ID", userId.ToString());
            client.DefaultRequestHeaders.Add("X-USER-ROLE", role.ToString());

            return client;
        }

        public static HttpClient GetAuthenticatedClient<TEntryPoint>(
            this WebApplicationFactory<TEntryPoint> factory,
            User user) where TEntryPoint : class
        {
            var client = factory.GetClient();

            client.DefaultRequestHeaders.Add("X-USER-ID", user.Id.ToString());
            client.DefaultRequestHeaders.Add("X-USER-ROLE", user.Role.ToString());

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
