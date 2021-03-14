using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Data.EF
{
    public static class EntityFrameworkRegistrar
    {
        public static void AddEntityFramework(
            this IServiceCollection services, DatabaseSettings settings)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(settings.ConnectionString, b =>
                {
                    b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            });

            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
        }
    }
}
