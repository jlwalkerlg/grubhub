using System;
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

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(280);

            builder.OwnsOne(x => x.Price, y =>
            {
                y.Ignore(z => z.Pence);

                y.Property(z => z.Pounds)
                    .HasColumnName("price")
                    .IsRequired();
            });

            builder.Property<Guid>("menu_category_id")
                .IsRequired();

            builder.Property<bool>("isDeleted")
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
        }
    }
}
