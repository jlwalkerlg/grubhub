using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace Web.Data.EF.Configurations
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.ToTable("restaurants");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(
                    x => x.Value,
                    x => new RestaurantId(x))
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.HasOne<RestaurantManager>()
                .WithMany()
                .HasForeignKey(x => x.ManagerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.ManagerId)
                .HasColumnName("manager_id");
            builder.HasIndex(x => x.ManagerId)
                .IsUnique();

            builder.Property(x => x.Name).IsRequired().HasColumnName("name");

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(400);

            builder.OwnsOne(x => x.PhoneNumber, x =>
            {
                x.Property(y => y.Number).IsRequired().HasColumnName("phone_number");
            });

            builder.OwnsOne(x => x.Address, x =>
            {
                x.Property(y => y.Value).IsRequired().HasColumnName("address");
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

            builder.OwnsOne(x => x.OpeningTimes, x =>
            {
                x.OwnsOne(y => y.Monday, y =>
                {
                    y.Property(z => z.Open).HasColumnName("monday_open");
                    y.Property(z => z.Close).HasColumnName("monday_close");
                });
                x.OwnsOne(y => y.Tuesday, y =>
                {
                    y.Property(z => z.Open).HasColumnName("tuesday_open");
                    y.Property(z => z.Close).HasColumnName("tuesday_close");
                });
                x.OwnsOne(y => y.Wednesday, y =>
                {
                    y.Property(z => z.Open).HasColumnName("wednesday_open");
                    y.Property(z => z.Close).HasColumnName("wednesday_close");
                });
                x.OwnsOne(y => y.Thursday, y =>
                {
                    y.Property(z => z.Open).HasColumnName("thursday_open");
                    y.Property(z => z.Close).HasColumnName("thursday_close");
                });
                x.OwnsOne(y => y.Friday, y =>
                {
                    y.Property(z => z.Open).HasColumnName("friday_open");
                    y.Property(z => z.Close).HasColumnName("friday_close");
                });
                x.OwnsOne(y => y.Saturday, y =>
                {
                    y.Property(z => z.Open).HasColumnName("saturday_open");
                    y.Property(z => z.Close).HasColumnName("saturday_close");
                });
                x.OwnsOne(y => y.Sunday, y =>
                {
                    y.Property(z => z.Open).HasColumnName("sunday_open");
                    y.Property(z => z.Close).HasColumnName("sunday_close");
                });
            });

            builder.OwnsOne(x => x.DeliveryFee, x =>
            {
                x.Property(y => y.Pence)
                    .IsRequired()
                    .HasColumnName("delivery_fee");
            });

            builder.OwnsOne(x => x.MinimumDeliverySpend, x =>
            {
                x.Property(y => y.Pence)
                    .IsRequired()
                    .HasColumnName("minimum_delivery_spend");
            });

            builder.Property(x => x.MaxDeliveryDistanceInKm)
                .IsRequired()
                .HasColumnName("max_delivery_distance_in_km");

            builder.Property(x => x.EstimatedDeliveryTimeInMinutes)
                .IsRequired()
                .HasColumnName("estimated_delivery_time_in_minutes");
        }
    }
}
