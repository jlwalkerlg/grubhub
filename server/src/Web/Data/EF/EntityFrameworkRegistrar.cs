using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Data.EF
{
    public static class EntityFrameworkRegistrar
    {
        public static void AddEntityFramework(
            this IServiceCollection services, Config config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(config.DbConnectionString, b =>
                {
                    b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            });

            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
        }
    }
}
