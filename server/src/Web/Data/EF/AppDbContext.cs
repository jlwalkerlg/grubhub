using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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
        public DbSet<RestaurantManager> RestaurantManagers { get; protected set; }
        public DbSet<User> Users { get; protected set; }
        public DbSet<Cuisine> Cuisines { get; protected set; }
        public DbSet<Basket> Baskets { get; protected set; }
        public DbSet<Order> Orders { get; protected set; }
        public DbSet<BillingAccount> BillingAccounts { get; protected set; }
        public DbSet<SerialisedEvent> Events { get; protected set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            DoSoftDeletes();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            DoSoftDeletes();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void DoSoftDeletes()
        {
            foreach (var entry in ChangeTracker.Entries<MenuCategory>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["isDeleted"] = true;
                }
            }

            foreach (var entry in ChangeTracker.Entries<MenuItem>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["isDeleted"] = true;
                }
            }
        }
    }
}
