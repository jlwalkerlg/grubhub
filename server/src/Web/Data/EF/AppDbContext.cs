using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
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
        private readonly IDistributedCache cache;

        public AppDbContext(DbContextOptions<AppDbContext> options, IDistributedCache cache) : base(options)
        {
            this.cache = cache;
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

        private async Task DoPreCommitActions()
        {
            DoSoftDeletes();
            await BustRestaurantCache();
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

        private async Task BustRestaurantCache()
        {
            var ids = new HashSet<RestaurantId>();

            foreach (var entry in ChangeTracker.Entries<Restaurant>())
            {
                if (ids.Contains(entry.Entity.Id)) continue;

                if (entry.State is EntityState.Modified or EntityState.Deleted)
                {
                    ids.Add(entry.Entity.Id);
                }
            }

            var menuCategoryEntries = ChangeTracker.Entries<MenuCategory>().ToDictionary(x => x.Entity);
            var menuItemEntries = ChangeTracker.Entries<MenuItem>().ToDictionary(x => x.Entity);

            foreach (var entry in ChangeTracker.Entries<Menu>())
            {
                if (ids.Contains(entry.Entity.RestaurantId)) continue;

                if (entry.State is EntityState.Modified or EntityState.Deleted)
                {
                    ids.Add(entry.Entity.RestaurantId);
                    continue;
                }

                if (entry.Entity.Categories.Any(category =>
                    menuCategoryEntries[category].State is EntityState.Modified or EntityState.Deleted))
                {
                    ids.Add(entry.Entity.RestaurantId);
                    continue;
                }

                if (entry.Entity.Categories.SelectMany(x => x.Items).Any(item =>
                    menuItemEntries[item].State is EntityState.Modified or EntityState.Deleted))
                {
                    ids.Add(entry.Entity.RestaurantId);
                }
            }

            foreach (var id in ids)
            {
                var key = $"restaurant:{id.Value}";
                await cache.RemoveAsync(key);
            }
        }
    }
}
