using Microsoft.EntityFrameworkCore;

namespace Web.Data.EF.Configurations
{
    public static class EventConfiguration
    {
        public static void ConfigureEvents(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SerialisedEvent>(builder =>
            {
                builder.ToTable("events");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.Id)
                    .HasColumnName("id");

                builder.Property(x => x.OccuredAt)
                    .HasColumnName("occured_at")
                    .IsRequired();

                builder.Property(x => x.Type)
                    .HasColumnName("type")
                    .IsRequired();

                builder.Property(x => x.Json)
                    .HasColumnName("json")
                    .HasColumnType("jsonb")
                    .IsRequired();
            });
        }
    }
}
