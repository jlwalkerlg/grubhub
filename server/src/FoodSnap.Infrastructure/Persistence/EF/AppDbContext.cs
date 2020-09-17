using System.Reflection;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace FoodSnap.Infrastructure.Persistence.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; protected set; }
        public DbSet<EventDto> Events { get; protected set; }
        public DbSet<RestaurantManager> RestaurantManagers { get; protected set; }
        public DbSet<User> Users { get; protected set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
