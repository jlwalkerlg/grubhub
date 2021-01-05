using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Domain.Menus;

namespace Web.Data.EF.Configurations
{
    public class MenuCategoryConfiguration : IEntityTypeConfiguration<MenuCategory>
    {
        public void Configure(EntityTypeBuilder<MenuCategory> builder)
        {
            builder.ToTable("menu_categories");

            builder.Property<int>("id")
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            builder.HasKey("id");

            builder.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey("menu_category_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.Property<int>("menu_id")
                .IsRequired();
            builder.HasIndex("menu_id", "Name")
                .IsUnique();
        }
    }
}
