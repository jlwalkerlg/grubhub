using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Domain.Menus;
using Web.Domain.Baskets;

namespace Web.Data.EF.Configurations
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.ToTable("basket_items");

            builder.Property<int>("id")
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            builder.HasKey("id");

            builder.HasOne<MenuItem>()
                .WithMany()
                .HasForeignKey(x => x.MenuItemId);

            builder.Property(x => x.MenuItemId)
                .IsRequired()
                .HasColumnName("menu_item_id");

            builder.Property(x => x.Quantity)
                .IsRequired()
                .HasColumnName("quantity");
        }
    }
}
