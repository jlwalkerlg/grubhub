using FoodSnap.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodSnap.Infrastructure.Persistence.EF.Configurations.UserConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasDiscriminator<string>("UserType")
                .HasValue<RestaurantManager>("RestaurantManager");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Name).IsRequired();

            builder.OwnsOne(x => x.Email, y =>
            {
                y.Property(x => x.Address).IsRequired().HasColumnName("Email");
            });

            builder.Property(x => x.Password).IsRequired();
        }
    }
}
