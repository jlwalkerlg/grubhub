using System;
using FoodSnap.Domain.Restaurants;
using Xunit;

namespace FoodSnap.DomainTests.Restaurants
{
    public class CoordinatesTests
    {
        [Theory]
        [InlineData(91)]
        [InlineData(-91)]
        public void Disallows_Invalid_Latitudes(float latitude)
        {
            Assert.Throws<ArgumentException>(() => new Coordinates(latitude, 0));
        }

        [Theory]
        [InlineData(-181)]
        [InlineData(81)]
        public void Disallows_Invalid_Longitudes(float longitude)
        {
            Assert.Throws<ArgumentException>(() => new Coordinates(0, longitude));
        }
    }
}
