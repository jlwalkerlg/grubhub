using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Web.Data.EF.Configurations;
using Web.Domain.Baskets;
using Web.Domain.Billing;
using Web.Domain.Cuisines;
using Web.Domain.Menus;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace Web.Data.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; protected set; }
        public DbSet<Menu> Menus { get; protected set; }
        public DbSet<User> Users { get; protected set; }
        public DbSet<Cuisine> Cuisines { get; protected set; }
        public DbSet<Basket> Baskets { get; protected set; }
        public DbSet<Order> Orders { get; protected set; }
        public DbSet<BillingAccount> BillingAccounts { get; protected set; }
        public DbSet<SerialisedEvent> Events { get; protected set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ConfigureBasketAggregate();
            builder.ConfigureBillingAccountAggregate();
            builder.ConfigureCuisineAggregate();
            builder.ConfigureMenuAggregate();
            builder.ConfigureOrderAggregate();
            builder.ConfigureRestaurantAggregate();
            builder.ConfigureEvents();
            builder.ConfigureUserAggregate();
        }

        public override int SaveChanges()
        {
            DoPreCommitActions().Wait();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await DoPreCommitActions();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private Task DoPreCommitActions()
        {
            DoSoftDeletes();
            return Task.CompletedTask;
        }

        private void DoSoftDeletes()
        {
            foreach (var entry in ChangeTracker.Entries<MenuCategory>())
            {
                if (entry.State is EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["isDeleted"] = true;
                }
            }

            foreach (var entry in ChangeTracker.Entries<MenuItem>())
            {
                if (entry.State is EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["isDeleted"] = true;
                }
            }
        }
    }
}
