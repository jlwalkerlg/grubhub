using System;
using Microsoft.EntityFrameworkCore;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;

namespace Web.Data.EF.Configurations
{
    public static class MenuConfiguration
    {
        public static void ConfigureMenuAggregate(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>(builder =>
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
            });

            modelBuilder.Entity<MenuCategory>(builder =>
            {
                builder.ToTable("menu_categories");

                builder.HasKey(x => x.Id);
                builder.Property(x => x.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                builder.HasMany(x => x.Items)
                    .WithOne()
                    .HasForeignKey("menu_category_id")
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Property(x => x.Name)
                    .HasColumnName("name")
                    .IsRequired();

                builder.Property<int>("menu_id")
                    .IsRequired();
            });

            modelBuilder.Entity<MenuItem>(builder =>
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

                builder.Property(x => x.Price)
                    .HasConversion(
                        price => price.Pence,
                        pence => Money.FromPence(pence))
                    .HasColumnName("price")
                    .IsRequired();

                builder.Property<Guid>("menu_category_id")
                    .IsRequired();
            });
        }
    }
}
