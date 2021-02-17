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

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("handled")]
        public bool Handled { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("json")]
        public string Json { get; set; }

        public Web.Features.Events.Event ToEvent()
        {
            return (Web.Features.Events.Event)JsonSerializer.Deserialize(
                Json,
                System.Type.GetType(Type));
        }
    }
}
