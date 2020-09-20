using FoodSnap.Domain.Menus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodSnap.Infrastructure.Persistence.EF.Configurations
{
    public class MenuCategoryConfiguration : IEntityTypeConfiguration<MenuCategory>
    {
        public void Configure(EntityTypeBuilder<MenuCategory> builder)
        {
            builder.ToTable("menu_categories");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();

            builder.HasMany<MenuItem>("items")
                .WithOne()
                .HasForeignKey("menu_category_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();
        }
    }
}
