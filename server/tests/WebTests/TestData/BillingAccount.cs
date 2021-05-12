using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTests.TestData
{
    [Table("billing_accounts")]
    public class BillingAccount
    {
        [Column("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("billing_enabled")]
        public bool IsBillingEnabled { get; set; } = true;
    }
}
