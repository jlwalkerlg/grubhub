using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;

namespace Web.Services.Jobs
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

            services.AddScoped<QuartzJobProcessor>();
            services.AddScoped<IJobQueue, QuartzJobQueue>();

            services.AddQuartzHostedService(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });
        }
    }
}
