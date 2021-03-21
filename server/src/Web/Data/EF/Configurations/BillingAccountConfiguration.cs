using Microsoft.EntityFrameworkCore;
using Web.Domain.Billing;
using Web.Domain.Restaurants;

namespace Web.Data.EF.Configurations
{
    public static class BillingAccountConfiguration
    {
        public static void ConfigureBillingAccountAggregate(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillingAccount>(builder =>
            {
                builder.ToTable("billing_accounts");

                builder.HasKey(x => x.Id);
                builder.Property(x => x.Id)
                    .HasConversion(
                        x => x.Value,
                        x => new BillingAccountId(x))
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                builder.HasOne<Restaurant>()
                    .WithMany()
                    .HasForeignKey(x => x.RestaurantId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                builder.Property(x => x.RestaurantId)
                    .HasColumnName("restaurant_id");
                builder.HasIndex(x => x.RestaurantId)
                    .IsUnique();

                builder.Property(x => x.Enabled)
                    .HasColumnName("billing_enabled")
                    .IsRequired();
            });
        }
    }
}
