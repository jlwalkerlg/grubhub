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

                builder.Property<int>("id")
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();
                builder.HasKey("id");

                builder.Property(x => x.Id)
                    .HasConversion(
                        x => x.Value,
                        x => new BillingAccountId(x))
                    .HasColumnName("account_id")
                    .ValueGeneratedNever();

                builder.Property(x => x.RestaurantId)
                    .HasConversion(
                        x => x.Value,
                        x => new RestaurantId(x))
                    .HasColumnName("restaurant_id")
                    .IsRequired();

                builder.Property(x => x.Enabled)
                    .HasColumnName("billing_enabled")
                    .IsRequired();
            });
        }
    }
}
