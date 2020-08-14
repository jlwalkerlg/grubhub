using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodSnap.Infrastructure.Persistence.EF.Configurations
{
    public class EventDtoConfiguration : IEntityTypeConfiguration<EventDto>
    {
        public void Configure(EntityTypeBuilder<EventDto> builder)
        {
            builder.ToTable("Events");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EventType).IsRequired().HasColumnName("Type");

            builder.Property(x => x.Data).IsRequired().HasColumnName("Data").HasColumnType("jsonb");

            builder.Property(x => x.CreatedAt).IsRequired().HasColumnName("CreatedAt");
        }
    }
}
