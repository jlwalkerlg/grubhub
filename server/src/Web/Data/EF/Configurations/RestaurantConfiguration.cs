using Microsoft.EntityFrameworkCore;
using Web.Domain;
using Web.Domain.Billing;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace Web.Data.EF.Configurations
{
    public static class RestaurantConfiguration
    {
        public static void ConfigureRestaurantAggregate(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>(builder =>
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

                builder.Property(x => x.Name)
                    .HasColumnName("name")
                    .IsRequired();

                builder.Property(x => x.Description)
                    .HasColumnName("description")
                    .HasMaxLength(400);

                builder.Property(x => x.PhoneNumber)
                    .HasConversion(
                        number => number.Number,
                        number => new PhoneNumber(number))
                    .HasColumnName("phone_number")
                    .IsRequired();

                builder.OwnsOne(x => x.Address, x =>
                {
                    x.Property(y => y.Line1)
                        .HasColumnName("address_line1")
                        .IsRequired();
                    x.Property(y => y.Line2)
                        .HasColumnName("address_line2");
                    x.Property(y => y.City)
                        .HasColumnName("city")
                        .IsRequired();
                    x.Property(y => y.Postcode)
                        .HasConversion(
                            postcode => postcode.Value,
                            postcode => new Postcode(postcode))
                        .HasColumnName("postcode")
                        .IsRequired();
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

                builder.Property(x => x.DeliveryFee)
                    .HasConversion(
                        fee => fee.Pence,
                        pence => Money.FromPence(pence))
                    .HasColumnName("delivery_fee")
                    .IsRequired();

                builder.Property(x => x.MinimumDeliverySpend)
                    .HasConversion(
                        spend => spend.Pence,
                        pence => Money.FromPence(pence))
                    .HasColumnName("minimum_delivery_spend")
                    .IsRequired();

                builder.Property(x => x.MaxDeliveryDistance)
                    .HasConversion(
                        distance => distance.Km,
                        km => Distance.FromKm(km))
                    .HasColumnName("max_delivery_distance_in_km")
                    .IsRequired();

                builder.Property(x => x.EstimatedDeliveryTimeInMinutes)
                    .IsRequired()
                    .HasColumnName("estimated_delivery_time_in_minutes");

                builder.Property(x => x.Thumbnail)
                    .HasColumnName("thumbnail");

                builder.Property(x => x.Banner)
                    .HasColumnName("banner");

                builder.Property(x => x.BillingAccountId)
                    .HasConversion(
                        x => x.Value,
                        x => string.IsNullOrWhiteSpace(x) ? null : new BillingAccountId(x))
                    .HasColumnName("billing_account_id");
            });
        }
    }
}
