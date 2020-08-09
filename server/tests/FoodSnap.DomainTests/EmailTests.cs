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
    }
}
