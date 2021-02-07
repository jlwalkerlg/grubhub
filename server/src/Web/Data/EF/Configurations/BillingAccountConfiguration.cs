using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Domain.Billing;
using Web.Domain.Restaurants;

namespace Web.Data.EF.Configurations
{
    public class BillingAccountConfiguration : IEntityTypeConfiguration<BillingAccount>
    {
        public void Configure(EntityTypeBuilder<BillingAccount> builder)
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
        }
    }
}
