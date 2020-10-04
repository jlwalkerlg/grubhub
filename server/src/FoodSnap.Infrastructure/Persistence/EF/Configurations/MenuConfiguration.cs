using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodSnap.Infrastructure.Persistence.EF.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("menus");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(
                    x => x.Value,
                    x => new MenuId(x))
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.HasOne<Restaurant>()
                .WithMany()
                .HasForeignKey(x => x.RestaurantId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.RestaurantId)
                .HasColumnName("restaurant_id");
            builder.HasIndex(x => x.RestaurantId)
                .IsUnique();

            builder.HasMany<MenuCategory>("categories")
                .WithOne()
                .HasForeignKey("menu_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
