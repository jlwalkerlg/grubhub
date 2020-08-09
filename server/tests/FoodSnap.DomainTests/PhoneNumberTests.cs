using System;
using FoodSnap.Domain;
using Xunit;

namespace FoodSnap.DomainTests
{
    public class PhoneNumberTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1")]
        public void Disallows_Invalid_Numbers(string number)
        {
            Assert.Throws<ArgumentException>(() => new PhoneNumber(number));
        }

        [Theory]
        [InlineData("01234 567890")]
        [InlineData("01234567890")]
        public void Allow_Valid_Numbers(string number)
        {
            new PhoneNumber(number);
        }
    }
}
