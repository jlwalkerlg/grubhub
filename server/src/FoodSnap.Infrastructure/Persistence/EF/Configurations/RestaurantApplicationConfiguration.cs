using FoodSnap.Domain.Restaurants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodSnap.Infrastructure.Persistence.EF.Configurations
{
    public class RestaurantApplicationConfiguration : IEntityTypeConfiguration<RestaurantApplication>
    {
        public void Configure(EntityTypeBuilder<RestaurantApplication> builder)
        {
            builder.ToTable("RestaurantApplications");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.RestaurantId).IsRequired().HasColumnName("RestaurantId");
            builder.HasOne<Restaurant>().WithMany().HasForeignKey(x => x.RestaurantId);
        }
    }
}
