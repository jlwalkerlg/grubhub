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
    }
}
