using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodSnap.Infrastructure.Persistence.EF.Configurations
{
    public class EventDtoConfiguration : IEntityTypeConfiguration<EventDto>
    {
        public void Configure(EntityTypeBuilder<EventDto> builder)
        {
            builder.ToTable("events");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(x => x.EventType).IsRequired().HasColumnName("type");

            builder.Property(x => x.Data).IsRequired().HasColumnName("data").HasColumnType("jsonb");

            builder.Property(x => x.CreatedAt).IsRequired().HasColumnName("created_at");
        }
    }
}
