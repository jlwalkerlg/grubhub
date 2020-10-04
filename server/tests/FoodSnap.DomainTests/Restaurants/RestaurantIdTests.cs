using System;
using FoodSnap.Domain.Restaurants;
using Xunit;

namespace FoodSnap.DomainTests.Restaurants
{
    public class RestaurantIdTests
    {
        [Fact]
        public void Test_Equality()
        {
            var id1 = new RestaurantId(Guid.NewGuid());
            var id2 = new RestaurantId(Guid.NewGuid());
            var id3 = new RestaurantId(id1.Value);

            Assert.NotEqual(id1, id2);
            Assert.False(id1 == id2);
            Assert.False(id1.Equals(id2));

            Assert.Equal(id1, id3);
            Assert.True(id1 == id3);
            Assert.True(id1.Equals(id3));
        }
    }
}
