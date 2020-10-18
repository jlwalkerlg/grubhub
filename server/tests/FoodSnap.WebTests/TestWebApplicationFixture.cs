using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Domain.Users;
using FoodSnap.Infrastructure.Persistence.EF;
using FoodSnap.Web;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Services.Tokenization;
using FoodSnap.WebTests.Doubles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Respawn;
using Xunit;

namespace FoodSnap.WebTests
{
    [CollectionDefinition(nameof(TestWebApplicationFixture))]
    public class TestWebApplicationFixtureCollection : ICollectionFixture<TestWebApplicationFixture>
    {
    }

    public class TestWebApplicationFixture
    {
        private readonly TestWebApplicationFactory factory;
        private readonly HttpClient client;
        private readonly WebConfig config;
        private readonly Checkpoint checkpoint;
        private string authToken = null;

        public TestWebApplicationFixture()
        {
            factory = new TestWebApplicationFactory();

            client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            config = factory.Services.GetRequiredService<WebConfig>();

            using (var db = factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>())
            {
                db.Database.EnsureCreated();
            }

            checkpoint = new Checkpoint
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[]
                {
                    "public"
                },
            };
        }

        public async Task ResetDatabase()
        {
            using (var conn = new NpgsqlConnection(config.DbConnectionString))
            {
                await conn.OpenAsync();
                await checkpoint.Reset(conn);
            }
        }

        public async Task ExecuteScope(Func<IServiceScope, Task> action)
        {
            using (var scope = factory.Services.CreateScope())
            {
                await action(scope);
            }
        }

        public async Task ExecuteDb(Func<AppDbContext, Task> action)
        {
            await ExecuteScope(async scope =>
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await action(db);
                await db.SaveChangesAsync();
            });
        }

        private Task<HttpResponseMessage> Send(HttpRequestMessage message)
        {
            if (authToken != null)
            {
                message.Headers.Add("Cookie", $"auth_token={authToken}");
            }
            return client.SendAsync(message);
        }

        public Task<HttpResponseMessage> Get(string uri)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, uri);
            return Send(message);
        }

        public async Task<T> Get<T>(string uri)
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

        public Task<HttpResponseMessage> Post(string uri, object data = null)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, uri);
            if (data != null)
            {
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            return Send(message);
        }

        public void Login(User user)
        {
            using (var scope = factory.Services.CreateScope())
            {
                var tokenizer = scope.ServiceProvider.GetRequiredService<ITokenizer>();
                authToken = tokenizer.Encode(user.Id.Value.ToString());
            }
        }
    }

    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseServiceProviderFactory(new TestWebAppServiceProviderFactory());
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices((ctx, services) =>
            {
                // Suppress info log.
                services.Configure<ConsoleLifetimeOptions>(options =>
                    options.SuppressStatusMessages = true);

                var sp = services.BuildServiceProvider();

                var config = sp.GetRequiredService<WebConfig>();
                config.DbConnectionString = ctx.Configuration["TestDbConnectionString"];
            });
        }

        private void RemoveService<T>(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }
    }

    public class TestWebAppServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
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

        private static void ConfigureContainer(ContainerBuilder builder)
        {
            builder
                .RegisterType<GeocoderStub>()
                .As<IGeocoder>()
                .SingleInstance();
        }
    }
}
