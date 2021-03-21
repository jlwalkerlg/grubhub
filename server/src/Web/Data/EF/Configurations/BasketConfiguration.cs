using Microsoft.EntityFrameworkCore;
using Web.Domain.Baskets;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace Web.Data.EF.Configurations
{
    public static class BasketConfiguration
    {
        public static void ConfigureBasketAggregate(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Basket>(builder =>
            {
                builder.ToTable("baskets");

                builder.Property<int>("id")
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();
                builder.HasKey("id");

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

                builder.HasMany(x => x.Items)
                    .WithOne()
                    .HasForeignKey("basket_id")
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasIndex(x => new { x.UserId, x.RestaurantId })
                    .IsUnique();
            });

            modelBuilder.Entity<BasketItem>(builder =>
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
            });
        }
    }
}
