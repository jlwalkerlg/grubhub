using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodSnap.Infrastructure.Persistence.EF.Configurations.RestaurantManagerConfiguration
{
    public class RestaurantManagerConfiguration : IEntityTypeConfiguration<RestaurantManager>
    {
        public void Configure(EntityTypeBuilder<RestaurantManager> builder)
        {
            builder.HasBaseType<User>();

            builder.Property(x => x.RestaurantId).IsRequired();

            // TODO: can't add foreign key as the table-per-hierarchy implementation of
            // inheritance used by ef core means that other sub-classes will fail the
            // foreign key constraint. Use separate tables (manually)?
            // builder.HasOne<Restaurant>().WithMany().HasForeignKey(x => x.RestaurantId);
        }
    }
}
