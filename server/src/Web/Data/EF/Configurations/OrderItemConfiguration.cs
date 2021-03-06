using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Orders;

namespace Web.Data.EF.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_items");

            builder.Property<int>("id")
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            builder.HasKey("id");

            builder.Property<OrderId>("order_id")
                .HasConversion(
                    x => x.Value,
                    x => new OrderId(x))
                .IsRequired();

            builder.HasOne<MenuItem>()
                .WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasForeignKey(x => x.MenuItemId);

            builder.Property(x => x.MenuItemId)
                .HasColumnName("menu_item_id");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnName("name");

            builder.Property(x => x.Price)
                .HasConversion(
                    price => price.Pounds,
                    pounds => Money.FromPounds(pounds))
                .HasColumnName("price")
                .IsRequired();

            builder.Property(x => x.Quantity)
                .IsRequired()
                .HasColumnName("quantity");
        }
    }
}
