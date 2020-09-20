using System;
using FoodSnap.Domain;
using Xunit;

namespace FoodSnap.DomainTests
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

        [Fact]
        public void Equal_When_Latitude_Longitude_Are_The_Same()
        {
            var coordinates1 = new Coordinates(0, 0);
            var coordinates2 = new Coordinates(coordinates1.Latitude, coordinates1.Longitude);

            Assert.Equal(coordinates1, coordinates2);
            Assert.True(coordinates1 == coordinates2);
            Assert.Equal(coordinates1.GetHashCode(), coordinates2.GetHashCode());
        }
    }
}
