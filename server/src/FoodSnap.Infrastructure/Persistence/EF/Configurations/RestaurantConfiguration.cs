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
            builder.ToTable("restaurants");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();

            builder.HasOne<RestaurantManager>()
                .WithMany()
                .HasForeignKey(x => x.ManagerId);
            builder.Property(x => x.ManagerId).HasColumnName("manager_id");

            builder.Property(x => x.Name).IsRequired().HasColumnName("name");

            builder.OwnsOne(x => x.PhoneNumber, x =>
            {
                x.Property(y => y.Number).IsRequired().HasColumnName("phone_number");
            });

            builder.OwnsOne(x => x.Address, x =>
            {
                x.Property(y => y.Line1).IsRequired().HasColumnName("address_line1");
                x.Property(y => y.Line2).HasColumnName("address_line2");
                x.Property(y => y.Town).IsRequired().HasColumnName("town");
                x.OwnsOne(y => y.Postcode, y =>
                {
                    y.Property(p => p.Code).IsRequired().HasColumnName("postcode");
                });
            });

            builder.OwnsOne(x => x.Coordinates, x =>
            {
                x.Property(y => y.Latitude).IsRequired().HasColumnName("latitude");
                x.Property(y => y.Longitude).IsRequired().HasColumnName("longitude");
            });

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
