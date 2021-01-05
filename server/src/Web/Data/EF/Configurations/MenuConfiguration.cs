using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Domain.Menus;
using Web.Domain.Restaurants;

namespace Web.Data.EF.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("menus");

            builder.Property<int>("id")
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            builder.HasKey("id");

            builder.HasOne<Restaurant>()
                .WithMany()
                .HasForeignKey(x => x.RestaurantId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.RestaurantId)
                .HasColumnName("restaurant_id");
            builder.HasIndex(x => x.RestaurantId)
                .IsUnique();

            builder.HasMany(x => x.Categories)
                .WithOne()
                .HasForeignKey("menu_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
