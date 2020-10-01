using System;
using FoodSnap.Domain;
using Xunit;

namespace FoodSnap.DomainTests
{
    public class AddressTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Disallows_Invalid_Values(string value)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Address(value);
            });
        }

        [Fact]
        public void Equal_When_Values_Are_The_Same()
        {
            var address1 = new Address("1 Maine Road, Manchester, UK");
            var address2 = new Address("1 Maine Road, Manchester, UK");

            Assert.Equal(address1, address2);
            Assert.True(address1 == address2);
            Assert.Equal(address1.GetHashCode(), address2.GetHashCode());
        }
    }
}
