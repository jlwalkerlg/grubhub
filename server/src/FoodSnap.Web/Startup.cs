using Autofac;
using FoodSnap.Application;
using FoodSnap.Infrastructure.Persistence.EF;
using FoodSnap.Web.ServiceRegistration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FoodSnap.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddEntityFramework(Configuration);

            services.AddMediatR(typeof(Result).Assembly);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddPresenters();
            builder.AddErrorPresenters();
            builder.AddGeocoder(Configuration);
            builder.AddMiddleware();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using (var scope = app.ApplicationServices.CreateScope())
                using (var context = scope.ServiceProvider.GetService<AppDbContext>())
                {
                    context.Database.EnsureCreated();
                }
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
