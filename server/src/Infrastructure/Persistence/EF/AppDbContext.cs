using System.Reflection;
using Domain.Menus;
using Domain.Restaurants;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; protected set; }
        public DbSet<Menu> Menus { get; protected set; }
        public DbSet<EventDto> Events { get; protected set; }
        public DbSet<RestaurantManager> RestaurantManagers { get; protected set; }
        public DbSet<User> Users { get; protected set; }
        public DbSet<Cuisine> Cuisines { get; protected set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
