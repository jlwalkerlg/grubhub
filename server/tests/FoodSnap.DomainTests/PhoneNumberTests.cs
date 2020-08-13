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

        [Fact]
        public void Equal_When_Number_Is_The_Same()
        {
            var phoneNumber1 = new PhoneNumber("01234567890");
            var phoneNumber2 = new PhoneNumber(phoneNumber1.Number);

            Assert.Equal(phoneNumber1, phoneNumber2);
            Assert.True(phoneNumber1 == phoneNumber2);
            Assert.Equal(phoneNumber1.GetHashCode(), phoneNumber2.GetHashCode());
        }
    }
}
