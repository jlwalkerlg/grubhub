using Microsoft.EntityFrameworkCore;
using Web.Domain.Cuisines;
using Web.Domain.Restaurants;

namespace Web.Data.EF.Configurations
{
    public static class CuisineConfiguration
    {
        public static void ConfigureCuisineAggregate(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cuisine>(builder =>
            {
                builder.ToTable("cuisines");

                builder.HasKey(x => x.Name);
                builder.Property(x => x.Name).HasColumnName("name");

                builder
                    .HasMany<Restaurant>("Restaurants")
                    .WithMany(x => x.Cuisines)
                    .UsingEntity<RestaurantCuisine>(
                        j => j
                            .HasOne(x => x.Restaurant)
                            .WithMany()
                            .HasForeignKey(x => x.RestaurantId),
                        j => j
                            .HasOne(x => x.Cuisine)
                            .WithMany()
                            .HasForeignKey(x => x.CuisineName),
                        j =>
                        {
                            j.ToTable("restaurant_cuisines");

                            j.Property(x => x.RestaurantId)
                                .HasConversion(
                                    x => x.Value,
                                    x => new RestaurantId(x)
                                )
                                .HasColumnName("restaurant_id");

                            j.Property(x => x.CuisineName)
                                .HasColumnName("cuisine_name");

                            j.HasKey(x => new {x.RestaurantId, x.CuisineName});
                        }
                    );
            });
        }
    }

    public class RestaurantCuisine
    {
        public RestaurantId RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public string CuisineName { get; set; }
        public Cuisine Cuisine { get; set; }
    }
}
