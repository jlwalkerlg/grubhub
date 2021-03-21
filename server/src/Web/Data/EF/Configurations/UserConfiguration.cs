using Microsoft.EntityFrameworkCore;
using Web.Domain;
using Web.Domain.Users;

namespace Web.Data.EF.Configurations
{
    public static class UserConfiguration
    {
        public static void ConfigureUserAggregate(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("users");

                builder.HasDiscriminator<string>("role")
                    .HasValue<RestaurantManager>(UserRole.RestaurantManager.ToString())
                    .HasValue<Customer>(UserRole.Customer.ToString());

                builder.HasKey(x => x.Id);
                builder.Property(x => x.Id)
                    .HasConversion(
                        x => x.Value,
                        x => new UserId(x))
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                builder.Property(x => x.FirstName)
                    .HasColumnName("first_name")
                    .IsRequired();

                builder.Property(x => x.LastName)
                    .HasColumnName("last_name")
                    .IsRequired();

                builder.Property(x => x.Email)
                    .HasConversion(
                        email => email.Address,
                        address => new Email(address))
                    .HasColumnName("email")
                    .IsRequired();

                builder.HasIndex(x => x.Email).IsUnique();

                builder.Property(x => x.Password)
                    .HasColumnName("password")
                    .IsRequired();

                builder.Property(x => x.MobileNumber)
                    .HasConversion(
                        number => number.Value,
                        number => new MobileNumber(number))
                    .HasColumnName("mobile_number");

                builder.OwnsOne(x => x.DeliveryAddress, y =>
                {
                    y.Property(address => address.Line1)
                        .HasColumnName("address_line1")
                        .IsRequired();

                    y.Property(address => address.Line2)
                        .HasColumnName("address_line2");

                    y.Property(address => address.City)
                        .HasColumnName("city")
                        .IsRequired();

                    y.Property(address => address.Postcode)
                        .HasConversion(
                            postcode => postcode.Value,
                            postcode => new Postcode(postcode))
                        .HasColumnName("postcode")
                        .IsRequired();
                });
            });
        }
    }
}
