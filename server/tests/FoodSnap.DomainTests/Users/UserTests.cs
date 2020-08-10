using System;
using FoodSnap.Domain;
using Xunit;

namespace FoodSnap.DomainTests.Users
{
    public class UserTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Disallows_Invalid_Names(string name)
        {
            var email = new Email("valid@test.com");
            var password = "password123";

            Assert.Throws<ArgumentException>(() =>
            {
                new DummyUser(name, email, password);
            });
        }

        [Fact]
        public void Disallows_Null_Emails()
        {
            var name = "Chow Main";
            var password = "password123";

            Assert.Throws<ArgumentNullException>(() =>
            {
                new DummyUser(name, null, password);
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Disallows_Invalid_Passwords(string password)
        {
            var name = "Mr Wong";
            var email = new Email("valid@test.com");

            Assert.Throws<ArgumentException>(() =>
            {
                new DummyUser(name, email, password);
            });
        }
    }
}
