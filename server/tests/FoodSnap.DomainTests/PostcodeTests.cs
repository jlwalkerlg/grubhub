using System;
using FoodSnap.Domain;
using Xunit;

namespace FoodSnap.DomainTests
{
    public class PostcodeTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("blahblahblah")]
        public void Disallows_Invalid_Postcodes(string postcode)
        {
            Assert.Throws<ArgumentException>(() => new Postcode(postcode));
        }

        [Theory]
        [InlineData("WS12 1WS")]
        [InlineData("ws12 1ws")]
        [InlineData("WS121WS")]
        [InlineData("WS1 1WS")]
        public void Allow_Valid_Postcodes(string postcode)
        {
            new Postcode(postcode);
        }

        [Fact]
        public void Equal_When_Code_Is_The_Same()
        {
            var postcode1 = new Postcode("MN12 1NM");
            var postcode2 = new Postcode(postcode1.Code);

            Assert.Equal(postcode1, postcode2);
            Assert.True(postcode1 == postcode2);
            Assert.Equal(postcode1.GetHashCode(), postcode2.GetHashCode());
        }
    }
}
