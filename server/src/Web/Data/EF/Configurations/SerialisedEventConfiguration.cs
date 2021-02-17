using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Data.EF.Configurations
{
    public class SerialisedEventConfiguration : IEntityTypeConfiguration<SerialisedEvent>
    {
        public void Configure(EntityTypeBuilder<SerialisedEvent> builder)
        {
            builder.ToTable("events");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(x => x.Handled)
                .HasColumnName("handled")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.Type)
                .HasColumnName("type")
                .IsRequired();

            builder.Property(x => x.Json)
                .HasColumnName("json")
                .HasColumnType("jsonb")
                .IsRequired();
        }
    }
}
