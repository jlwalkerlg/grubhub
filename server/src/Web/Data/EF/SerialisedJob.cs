using System;

namespace Web.Data.EF
{
    public record SerialisedJob
    {
        public Guid Id { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int? MaxAttempts { get; set; }
        public int Attempts { get; set; }
        public string Type { get; set; }
        public string Json { get; set; }
    }
}
