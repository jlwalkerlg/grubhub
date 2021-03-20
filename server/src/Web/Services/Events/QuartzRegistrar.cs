using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;

namespace Web.Services.Events
{
    public static class QuartzRegistrar
    {
        public static void AddQuartz(this IServiceCollection services, DatabaseSettings settings)
        {
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                // Default name.
                q.SchedulerName = "QuartzScheduler";

                q.UsePersistentStore(store =>
                {
                    store.UsePostgres(settings.ConnectionString);

                    store.UseJsonSerializer();
                });
            });

            services.AddSingleton<IScheduler>(sp =>
                SchedulerRepository
                    .Instance
                    .Lookup("QuartzScheduler")
                    .Result);

            services.AddQuartzHostedService(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });
        }

        public static void AddQuartzEventBus(this IServiceCollection services)
        {
            services.AddQuartz();

            services.AddScoped<IEventBus, QuartzEventBus>();
            services.AddScoped<QuartzEventDispatcher>();

            foreach (var type in typeof(Program).Assembly.GetTypes())
            {
                var interfaces = type.GetInterfaces()
                    .Where(face => face.IsGenericType && face.GetGenericTypeDefinition() == typeof(IEventListener<>))
                    .ToList();

                if (!interfaces.Any()) continue;

                services.AddTransient(type);

                foreach (var face in interfaces)
                {
                    services.AddTransient(face, type);
                }
            }
        }
    }
}
