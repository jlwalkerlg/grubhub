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

            builder.Property(x => x.Retries)
                .HasColumnName("retries")
                .IsRequired();

            builder.Property(x => x.Attempts)
                .HasColumnName("attempts")
                .IsRequired();

            builder.Property(x => x.IsComplete)
                .HasColumnName("is_complete")
                .IsRequired();

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
