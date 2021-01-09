using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTests.TestData
{
    [Table("users")]
    public record User
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        public string Name { get; set; } = Guid.NewGuid().ToString();

        [Column("email")]
        public string Email { get; set; } = Guid.NewGuid().ToString() + "@gmail.com";

        [Column("password")]
        public string Password { get; set; } = Guid.NewGuid().ToString();

        [Column("role")]
        public string Role { get; set; } = "RestaurantManager";
    }
}
