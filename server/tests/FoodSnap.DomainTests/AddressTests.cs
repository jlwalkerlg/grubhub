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
        public void Disallows_Invalid_Line1s(string line1)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Address(line1, "", "Manchester", new Postcode("ws12 1ws"));
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Disallows_Invalid_Towns(string town)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Address("12 Manchester Road", "", town, new Postcode("ws12 1ws"));
            });
        }

        [Fact]
        public void Disallows_Null_Postcodes()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Address("12 Manchester Road", "", "Manchester", null);
            });
        }

        [Fact]
        public void Equal_When_Address_Components_Are_The_Same()
        {
            var address1 = new Address("19 Main Street", "Margate", "Manchester", new Postcode("MN12 1NM"));
            var address2 = new Address(address1.Line1, address1.Line2, address1.Town, address1.Postcode);

            Assert.Equal(address1, address2);
            Assert.True(address1 == address2);
            Assert.Equal(address1.GetHashCode(), address2.GetHashCode());
        }
    }
}
