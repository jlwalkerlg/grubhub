using System;
using FoodSnap.Domain;
using FoodSnap.Domain.Users;
using Xunit;

namespace FoodSnap.DomainTests.Users
{
    public class UserTests
    {
        [Fact]
        public void Test_Equality()
        {
            var u1 = new DummyUser(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var u2 = new DummyUser(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var u3 = new DummyUser(
                u1.Id,
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            Assert.NotEqual(u1, u2);
            Assert.False(u1 == u2);
            Assert.False(u1.Equals(u2));

            Assert.Equal(u1, u3);
            Assert.True(u1 == u3);
            Assert.True(u1.Equals(u3));
        }

        [Fact]
        public void Id_Cant_Be_Null()
        {
            var name = "Jordan Walker";
            var email = new Email("valid@test.com");
            var password = "password123";

            Assert.Throws<ArgumentNullException>(() =>
            {
                new DummyUser(null, name, email, password);
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Disallows_Invalid_Names(string name)
        {
            var id = new UserId(Guid.NewGuid());
            var email = new Email("valid@test.com");
            var password = "password123";

            Assert.Throws<ArgumentException>(() =>
            {
                new DummyUser(id, name, email, password);
            });
        }

        [Fact]
        public void Disallows_Null_Emails()
        {
            var id = new UserId(Guid.NewGuid());
            var name = "Chow Main";
            var password = "password123";

            Assert.Throws<ArgumentNullException>(() =>
            {
                new DummyUser(id, name, null, password);
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Disallows_Invalid_Passwords(string password)
        {
            var id = new UserId(Guid.NewGuid());
            var name = "Mr Wong";
            var email = new Email("valid@test.com");

            Assert.Throws<ArgumentException>(() =>
            {
                new DummyUser(id, name, email, password);
            });
        }
    }
}
