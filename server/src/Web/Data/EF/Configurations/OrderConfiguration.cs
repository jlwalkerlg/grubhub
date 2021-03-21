using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace Web.Data.EF.Configurations
{
    public static class OrderConfiguration
    {
        public static void ConfigureOrderAggregate(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(builder =>
            {
                builder.ToTable("orders");

                builder.Property<int>("number")
                    .HasColumnName("number")
                    .ValueGeneratedOnAdd();

                builder.HasKey(x => x.Id);
                builder.Property(x => x.Id)
                    .HasConversion(
                        x => x.Value,
                        x => new OrderId(x))
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                builder.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                builder.Property(x => x.UserId)
                    .HasColumnName("user_id");

                builder.HasOne<Restaurant>()
                    .WithMany()
                    .HasForeignKey(x => x.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                builder.Property(x => x.RestaurantId)
                    .HasColumnName("restaurant_id");

                builder.Property(x => x.DeliveryFee)
                    .HasConversion(
                        fee => fee.Pence,
                        pence => Money.FromPence(pence))
                    .HasColumnName("delivery_fee")
                    .IsRequired();

                builder.Property(x => x.ServiceFee)
                    .HasConversion(
                        fee => fee.Pence,
                        pence => Money.FromPence(pence))
                    .HasColumnName("service_fee")
                    .IsRequired();

                builder.Property(x => x.Status)
                    .HasColumnName("status")
                    .IsRequired()
                    .HasConversion(new EnumToStringConverter<OrderStatus>());

                builder.Property(x => x.MobileNumber)
                    .HasConversion(
                        number => number.Value,
                        number => new MobileNumber(number))
                    .HasColumnName("mobile_number")
                    .IsRequired();

                builder.OwnsOne(x => x.Address, x =>
                {
                    x.Property(y => y.Line1)
                        .HasColumnName("address_line1")
                        .IsRequired();
                    x.Property(y => y.Line2)
                        .HasColumnName("address_line2");
                    x.Property(y => y.City)
                        .HasColumnName("city")
                        .IsRequired();
                    x.Property(y => y.Postcode)
                        .HasConversion(
                            postcode => postcode.Value,
                            postcode => new Postcode(postcode))
                        .HasColumnName("postcode")
                        .IsRequired();
                });

                builder.Property(x => x.PlacedAt)
                    .HasColumnName("placed_at")
                    .IsRequired();

                builder.Property(x => x.ConfirmedAt)
                    .HasColumnName("confirmed_at");

                builder.Property(x => x.AcceptedAt)
                    .HasColumnName("accepted_at");

                builder.Property(x => x.DeliveredAt)
                    .HasColumnName("delivered_at");

                builder.Property(x => x.RejectedAt)
                    .HasColumnName("rejected_at");

                builder.Property(x => x.CancelledAt)
                    .HasColumnName("cancelled_at");

                builder.Property(x => x.PaymentIntentId)
                    .HasColumnName("payment_intent_id")
                    .IsRequired();

                builder.Property(x => x.PaymentIntentClientSecret)
                    .HasColumnName("payment_intent_client_secret")
                    .IsRequired();

                builder.HasMany(x => x.Items)
                    .WithOne()
                    .HasForeignKey("order_id")
                    .OnDelete(DeleteBehavior.Cascade);
                });

                modelBuilder.Entity<OrderItem>(builder =>
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
                            price => price.Pence,
                            pence => Money.FromPence(pence))
                        .HasColumnName("price")
                        .IsRequired();

                    builder.Property(x => x.Quantity)
                        .IsRequired()
                        .HasColumnName("quantity");
            });
        }
    }
}
