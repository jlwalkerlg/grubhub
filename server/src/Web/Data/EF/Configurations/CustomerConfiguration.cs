using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Domain;
using Web.Domain.Users;

namespace Web.Data.EF.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
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
        }
    }
}
