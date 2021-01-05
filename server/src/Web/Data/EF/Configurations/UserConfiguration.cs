using Web.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Data.EF.Configurations.UserConfiguration
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

            builder.Property(x => x.Name).IsRequired().HasColumnName("name");

            builder.OwnsOne(x => x.Email, y =>
            {
                y.Property(x => x.Address).IsRequired().HasColumnName("email");
                y.HasIndex(x => x.Address).IsUnique();
            });

            builder.Property(x => x.Password).IsRequired().HasColumnName("password");
        }
    }
}
