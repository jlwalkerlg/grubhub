using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace WebTests.TestData
{
    [Table("events")]
    public record Event
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("data")]
        public string Data { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public TEvent ToEvent<TEvent>() where TEvent : Web.Features.Events.Event
        {
            return JsonSerializer.Deserialize<TEvent>(Data);
        }
    }
}
