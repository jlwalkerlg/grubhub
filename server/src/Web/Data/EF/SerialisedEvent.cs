using System;

namespace Web.Data.EF
{
    public record SerialisedEvent
    {
        public long Id { get; set; }
        public DateTimeOffset OccuredAt { get; set; }
        public string Type { get; set; }
        public string Json { get; set; }
    }
}
