using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web;
using Web.Domain.Users;
using Web.Services;
using Web.Services.Tokenization;
using WebTests.Doubles;
using Xunit;

namespace WebTests
{
    [Trait("Category", "HttpTest")]
    public abstract class HttpTestBase : IClassFixture<HttpTestFixture>
    {
        protected readonly HttpTestFixture fixture;

        public HttpTestBase(HttpTestFixture fixture)
        {
            this.fixture = fixture;
        }
    }

    public class HttpTestFixture
    {
        protected readonly HttpTestWebApplicationFactory factory;
        protected readonly ITokenizer tokenizer;

        public string HandlerErrorMessage =>
            HttpTestWebApplicationFactory.FailRequestHandlerError;

        public HttpTestFixture() : this(new())
        {
        }

        protected HttpTestFixture(HttpTestWebApplicationFactory factory)
        {
            this.factory = factory;

            using (var scope = factory.Services.CreateScope())
            {
                tokenizer = scope.ServiceProvider
                    .GetRequiredService<ITokenizer>();
            }
        }

        public WebApplicationFactory<Startup> CreateFactory(Action<IServiceCollection> configureServices)
        {
            return factory.WithWebHostBuilder(config =>
            {
                config.ConfigureServices(configureServices);
            });
        }

        public HttpTestClient GetAuthenticatedClient()
        {
            return GetAuthenticatedClient(Guid.NewGuid().ToString());
        }

        public HttpTestClient GetAuthenticatedClient(UserId userId)
        {
            return GetAuthenticatedClient(userId.Value.ToString());
        }

        public HttpTestClient GetAuthenticatedClient(Guid userId)
        {
            return GetAuthenticatedClient(userId.ToString());
        }

        public HttpTestClient GetAuthenticatedClient(string userId)
        {
            return GetClient().Authenticate(userId);
        }

        public HttpTestClient GetClient()
        {
            var client = factory.CreateClient(
                new WebApplicationFactoryClientOptions()
                {
                    AllowAutoRedirect = false,
                    HandleCookies = true,
                });

            return new HttpTestClient(client, tokenizer);
        }
    }

    public class HttpTestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public static string FailRequestHandlerError = "Error in stubbed handler.";

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseServiceProviderFactory(new HttpTestServiceProviderFactory());
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IClock, ClockStub>();

                ConfigureServices(services);
            });
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            var handlers = typeof(Startup).Assembly
                .GetTypes()
                .Where(x => !x.IsInterface
                    && x.GetInterfaces().Any(
                        i => i.IsGenericType
                        && i.GetGenericTypeDefinition() == typeof(MediatR.IRequestHandler<,>)
                ));

            foreach (var handler in handlers)
            {
                var interfaces = handler.GetInterfaces()
                    .Where(x => x.IsGenericType
                        && x.GetGenericTypeDefinition() == typeof(MediatR.IRequestHandler<,>));

                foreach (var i in interfaces)
                {
                    var handlerStub = Activator.CreateInstance(
                        typeof(FailRequestHandlerStub<,>)
                            .MakeGenericType(i.GetGenericArguments()));

                    services.AddTransient(i, sp => handlerStub);
                }
            }
        }

        private class FailRequestHandlerStub<TRequest, TResponse>
            : MediatR.IRequestHandler<TRequest, TResponse>
            where TRequest : MediatR.IRequest<TResponse>
            where TResponse : Result, new()
        {
            public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new TResponse()
                {
                    Error = Error.BadRequest(FailRequestHandlerError)
                });
            }
        }
    }

    public class HttpTestServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
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

        private void ConfigureContainer(ContainerBuilder builder)
        {
            builder
                .RegisterType<GeocoderStub>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
