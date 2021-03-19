using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace WebTests.TestData
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<Cuisine> Cuisines { get; private set; }
        public DbSet<Menu> Menus { get; private set; }
        public DbSet<MenuCategory> MenuCategories { get; private set; }
        public DbSet<MenuItem> MenuItems { get; private set; }
        public DbSet<Restaurant> Restaurants { get; private set; }
        public DbSet<RestaurantCuisine> RestaurantCuisines { get; private set; }
        public DbSet<User> Users { get; private set; }
        public DbSet<Basket> Baskets { get; private set; }
        public DbSet<BasketItem> BasketItems { get; private set; }
        public DbSet<Order> Orders { get; private set; }
        public DbSet<OrderItem> OrderItems { get; private set; }
        public DbSet<BillingAccount> BillingAccounts { get; private set; }
        public DbSet<Event> Events { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cuisine>()
                .HasMany(p => p.Restaurants)
                .WithMany(p => p.Cuisines)
                .UsingEntity<RestaurantCuisine>(
                    j => j
                        .HasOne(pt => pt.Restaurant)
                        .WithMany()
                        .HasForeignKey(pt => pt.RestaurantId),
                    j => j
                        .HasOne(pt => pt.Cuisine)
                        .WithMany()
                        .HasForeignKey(pt => pt.CuisineName),
                    j =>
                    {
                        j.HasKey(x => new { x.RestaurantId, x.CuisineName });
                    });

            modelBuilder.Entity<User>()
                .Property(x => x.Role)
                .HasConversion(new EnumToStringConverter<UserRole>());

            modelBuilder.Entity<Restaurant>()
                .Property(x => x.Status)
                .HasConversion(new EnumToStringConverter<RestaurantStatus>());

            modelBuilder.Entity<Restaurant>()
                .Property(x => x.DeliveryFee)
                .HasConversion(
                    pounds => (int) (pounds * 100),
                    pence => ((decimal) pence) / 100);

            modelBuilder.Entity<Restaurant>()
                .Property(x => x.MinimumDeliverySpend)
                .HasConversion(
                    pounds => (int) (pounds * 100),
                    pence => ((decimal) pence) / 100);

            modelBuilder.Entity<OrderItem>()
                .Property(x => x.Price)
                .HasConversion(
                    pounds => (int) (pounds * 100),
                    pence => ((decimal) pence) / 100);

            modelBuilder.Entity<MenuItem>()
                .Property(x => x.Price)
                .HasConversion(
                    pounds => (int) (pounds * 100),
                    pence => ((decimal) pence) / 100);

            modelBuilder.Entity<Order>()
                .Property(x => x.Status)
                .HasConversion(new EnumToStringConverter<OrderStatus>());

            modelBuilder.Entity<Order>()
                .Property(x => x.DeliveryFee)
                .HasConversion(
                    pounds => (int) (pounds * 100),
                    pence => ((decimal) pence) / 100);

            modelBuilder.Entity<Order>()
                .Property(x => x.ServiceFee)
                .HasConversion(
                    pounds => (int) (pounds * 100),
                    pence => ((decimal) pence) / 100);
        }
    }
}
