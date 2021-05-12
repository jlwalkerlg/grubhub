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

                builder.Property(x => x.Id)
                    .HasConversion(
                        x => x.Value,
                        x => new BillingAccountId(x))
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                builder.Property(x => x.Enabled)
                    .HasColumnName("billing_enabled")
                    .IsRequired();

                builder.HasMany<Restaurant>()
                    .WithOne()
                    .HasForeignKey(x => x.BillingAccountId)
                    .OnDelete(DeleteBehavior.SetNull);

                builder.HasKey(x => x.Id);
            });
        }
    }
}
