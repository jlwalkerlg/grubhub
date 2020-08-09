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
    }
}
