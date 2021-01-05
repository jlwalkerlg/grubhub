using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Web;
using Web.Domain.Users;
using Web.Envelopes;
using Web.Services;
using Web.Services.Geocoding;
using Web.Services.Tokenization;
using WebTests.Doubles;

namespace WebTests
{
    public abstract class WebTestBase
    {
        private readonly WebTestFixture fixture;
        protected HttpClient client;
        private string authToken;

        public WebTestBase(WebTestFixture fixture)
        {
            this.fixture = fixture;

            client = fixture.Factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected async Task Login(Guid userId)
        {
            await fixture.ExecuteService<ITokenizer>(tokenizer =>
            {
                authToken = tokenizer.Encode(userId.ToString());
                return Task.CompletedTask;
            });
        }

        protected async Task Login()
        {
            await Login(Guid.NewGuid());
        }

        protected async Task Login(User user)
        {
            await Login(user.Id.Value);
        }

        protected void Logout()
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

    public class WebTestFixture
    {
        public virtual WebTestAppFactory Factory { get; } = new();

        public async Task ExecuteScope(Func<IServiceScope, Task> action)
        {
            using (var scope = Factory.Services.CreateScope())
            {
                await action(scope);
            }
        }

        public async Task ExecuteService<TService>(Func<TService, Task> action)
        {
            await ExecuteScope(async scope =>
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                await action(service);
            });
        }
    }

    public class WebTestAppFactory : WebApplicationFactory<Startup>
    {
        protected virtual WebTestServiceProviderFactory ServiceProviderFactory { get; } = new();

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseServiceProviderFactory(ServiceProviderFactory);
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices((ctx, services) =>
            {
                // Suppress info log.
                services.Configure<ConsoleLifetimeOptions>(options =>
                options.SuppressStatusMessages = true);
            });
        }

        protected void RemoveService<T>(IServiceCollection services)
        {
            var descriptor = services.Single(d => d.ServiceType == typeof(T));
            services.Remove(descriptor);
        }
    }

    public class WebTestServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            return builder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder builder)
        {
            ConfigureContainer(builder);

            return new AutofacServiceProvider(builder.Build());
        }

        protected virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder
                .RegisterType<GeocoderStub>()
                .As<IGeocoder>()
                .SingleInstance();

            builder
                .RegisterType<ClockStub>()
                .As<IClock>()
                .SingleInstance();
        }
    }
}
