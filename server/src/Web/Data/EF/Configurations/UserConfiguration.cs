using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Domain;
using Web.Domain.Users;

namespace Web.Data.EF.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasDiscriminator<string>("role")
                .HasValue<RestaurantManager>(UserRole.RestaurantManager.ToString());

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(
                    x => x.Value,
                    x => new UserId(x))
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.Name)
                .HasColumnName("name")
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
        }
    }
}
