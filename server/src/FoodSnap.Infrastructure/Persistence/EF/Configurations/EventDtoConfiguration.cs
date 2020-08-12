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

            builder.Property(x => x.Event).IsRequired().HasColumnType("jsonb").HasColumnName("Data");

            builder.Property(x => x.CreatedAt).IsRequired().HasColumnName("CreatedAt");
        }
    }
}
