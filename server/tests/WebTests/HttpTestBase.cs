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

        public string HandlerErrorMessage =>
            HttpTestWebApplicationFactory.FailRequestHandlerError;

        public HttpTestFixture() : this(new())
        {
        }

        protected HttpTestFixture(HttpTestWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        public IServiceProvider Services => factory.Services;

        public T GetService<T>()
        {
            using (var scope = factory.Services.CreateScope())
            {
                return scope.ServiceProvider.GetService<T>();
            }
        }

        public async Task<Result> Send(IRequest request)
        {
            using (var scope = factory.Services.CreateScope())
            {
                var sender = scope.ServiceProvider.GetRequiredService<MediatR.ISender>();
                return await sender.Send(request);
            }
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
            return new HttpTestClient(factory);
        }
    }

    public class HttpTestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public static string FailRequestHandlerError = "Error in stubbed handler.";

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder
                .UseServiceProviderFactory(new HttpTestServiceProviderFactory())
                .ConfigureWebHost(x => x.UseEnvironment("Testing"));

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
            services.AddTransient(
                typeof(MediatR.IPipelineBehavior<,>),
                typeof(BadRequestBehavior<,>));
        }

        private class BadRequestBehavior<TRequest, TResponse>
            : MediatR.IPipelineBehavior<TRequest, TResponse>
            where TResponse : Result, new()
        {
            public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, MediatR.RequestHandlerDelegate<TResponse> next)
            {
                return Task.FromResult(new TResponse()
                {
                    Error = Error.BadRequest(FailRequestHandlerError),
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
