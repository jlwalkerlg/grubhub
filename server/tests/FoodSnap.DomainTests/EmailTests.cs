using System;
using FoodSnap.Domain;
using Xunit;

namespace FoodSnap.DomainTests
{
    public class EmailTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("blahblahblah")]
        public void Disallows_Invalid_Addresses(string address)
        {
            Assert.Throws<ArgumentException>(() => new Email(address));
        }

        [Theory]
        [InlineData("valid@test.com")]
        public void Allow_Valid_Addresses(string address)
        {
            new Email(address);
        }

        [Fact]
        public void Equal_When_Address_Is_The_Same()
        {
            var email1 = new Email("test@email.com");
            var email2 = new Email(email1.Address);

            Assert.Equal(email1, email2);
            Assert.True(email1 == email2);
            Assert.Equal(email1.GetHashCode(), email2.GetHashCode());
        }
    }
}
