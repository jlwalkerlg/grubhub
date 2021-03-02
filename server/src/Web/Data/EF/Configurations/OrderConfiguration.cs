using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace Web.Data.EF.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
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
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.UserId)
                .HasColumnName("user_id");

            builder.HasOne<Restaurant>()
                .WithMany()
                .HasForeignKey(x => x.RestaurantId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.RestaurantId)
                .HasColumnName("restaurant_id");

            builder.OwnsOne(x => x.Subtotal, x =>
            {
                x.Ignore(y => y.Pence);

                x.Property(y => y.Pounds)
                    .HasColumnName("subtotal")
                    .IsRequired();
            });

            builder.OwnsOne(x => x.DeliveryFee, x =>
            {
                x.Ignore(y => y.Pence);

                x.Property(y => y.Pounds)
                    .HasColumnName("delivery_fee")
                    .IsRequired();
            });

            builder.OwnsOne(x => x.ServiceFee, x =>
            {
                x.Ignore(y => y.Pence);

                x.Property(y => y.Pounds)
                    .HasColumnName("service_fee")
                    .IsRequired();
            });

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired()
                .HasConversion(new EnumToStringConverter<OrderStatus>());

            builder.OwnsOne(x => x.MobileNumber, x =>
            {
                x.Property(y => y.Value)
                    .HasColumnName("mobile_number")
                    .IsRequired();
            });

            builder.OwnsOne(x => x.Address, x =>
            {
                x.Property(y => y.Value)
                    .HasColumnName("address")
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
        }
    }
}
