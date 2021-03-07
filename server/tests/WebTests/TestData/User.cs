using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Domain.Users;

namespace WebTests.TestData
{
    [Table("users")]
    public record User
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("first_name")]
        public string FirstName { get; set; } = Guid.NewGuid().ToString();

        [Column("last_name")]
        public string LastName { get; set; } = Guid.NewGuid().ToString();

        [Column("email")]
        public string Email { get; set; } = Guid.NewGuid() + "@gmail.com";

        [Column("password")]
        public string Password { get; set; } = Guid.NewGuid().ToString();

        [Column("role")]
        public UserRole Role { get; set; } = UserRole.RestaurantManager;

        [Column("mobile_number")]
        public string MobileNumber { get; set; }

        [Column("address_line1")]
        public string AddressLine1 { get; set; }

        [Column("address_line2")]
        public string AddressLine2 { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("postcode")]
        public string Postcode { get; set; }
    }
}
