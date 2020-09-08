using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bruno.Infrastructure.Persistence.EF.Configurations
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.ToTable("Restaurants");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.HasOne<RestaurantManager>()
                .WithMany()
                .HasForeignKey(x => x.ManagerId);

            builder.Property(x => x.Name).IsRequired();

            builder.OwnsOne(x => x.PhoneNumber, x =>
            {
                x.Property(y => y.Number).IsRequired().HasColumnName("PhoneNumber");
            });

            builder.OwnsOne(x => x.Address, x =>
            {
                x.Property(y => y.Line1).IsRequired().HasColumnName("AddressLine1");
                x.Property(y => y.Line2).HasColumnName("AddressLine2");
                x.Property(y => y.Town).IsRequired().HasColumnName("Town");
                x.OwnsOne(y => y.Postcode, y =>
                {
                    y.Property(p => p.Code).IsRequired().HasColumnName("Postcode");
                });
            });

            builder.OwnsOne(x => x.Coordinates, x =>
            {
                x.Property(y => y.Latitude).IsRequired().HasColumnName("Latitude");
                x.Property(y => y.Longitude).IsRequired().HasColumnName("Longitude");
            });

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
