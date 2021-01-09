using Microsoft.EntityFrameworkCore;

namespace WebTests.TestData
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<Cuisine> Cuisines { get; private set; }
        public DbSet<Event> Events { get; private set; }
        public DbSet<Menu> Menus { get; private set; }
        public DbSet<MenuCategory> MenuCategories { get; private set; }
        public DbSet<MenuItem> MenuItems { get; private set; }
        public DbSet<Restaurant> Restaurants { get; private set; }
        public DbSet<RestaurantCuisine> RestaurantCuisines { get; private set; }
        public DbSet<User> Users { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Cuisine>()
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
        }
    }
}
