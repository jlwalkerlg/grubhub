using System;
using FoodSnap.Domain.Users;
using Xunit;

namespace FoodSnap.DomainTests.Users
{
    public class UserIdTests
    {
        [Fact]
        public void Test_Equality()
        {
            var id1 = new UserId(Guid.NewGuid());
            var id2 = new UserId(Guid.NewGuid());
            var id3 = new UserId(id1.Value);

            Assert.NotEqual(id1, id2);
            Assert.False(id1 == id2);
            Assert.False(id1.Equals(id2));

            Assert.Equal(id1, id3);
            Assert.True(id1 == id3);
            Assert.True(id1.Equals(id3));
        }
    }
}
