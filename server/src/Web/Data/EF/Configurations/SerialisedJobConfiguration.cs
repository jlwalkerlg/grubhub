using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Data.EF.Configurations
{
    public class SerialisedJobConfiguration : IEntityTypeConfiguration<SerialisedJob>
    {
        public void Configure(EntityTypeBuilder<SerialisedJob> builder)
        {
            builder.ToTable("jobs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.ScheduledAt)
                .HasColumnName("scheduled_at")
                .IsRequired();

            builder.Property(x => x.MaxAttempts)
                .HasColumnName("max_attempts");

            builder.Property(x => x.Attempts)
                .HasColumnName("attempts")
                .IsRequired()
                .HasDefaultValue(0);

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
