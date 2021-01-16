using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Domain.Menus;

namespace Web.Data.EF.Configurations
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable("menu_items");

            builder.Property<int>("id")
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            builder.HasKey("id");

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(280);

            builder.OwnsOne(x => x.Price, y =>
            {
                y.Property(z => z.Amount)
                    .HasColumnName("price")
                    .IsRequired();
            });

            builder.Property<int>("menu_category_id")
                .IsRequired();
            builder.HasIndex("menu_category_id", "Name")
                .IsUnique();
        }
    }
}
