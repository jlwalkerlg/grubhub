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

        [Column("occured_at")]
        public DateTime OccuredAt { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("json")]
        public string Json { get; set; }

        public Web.Services.Events.Event ToEvent()
        {
            return (Web.Services.Events.Event)JsonSerializer.Deserialize(
                Json,
                System.Type.GetType(Type)!);
        }
    }
}
