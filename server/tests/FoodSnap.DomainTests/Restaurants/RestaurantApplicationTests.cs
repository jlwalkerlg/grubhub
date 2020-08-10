using System;
using FoodSnap.Domain.Restaurants;
using Xunit;

namespace FoodSnap.DomainTests.Restaurants
{
    public class RestaurantApplicationTests
    {
        [Fact]
        public void Disallows_Empty_Restaurant_Ids()
        {
            Assert.Throws<ArgumentException>(() => new RestaurantApplication(Guid.Empty));
        }

        [Fact]
        public void New_Applications_Are_Pending()
        {
            var application = new RestaurantApplication(Guid.NewGuid());

            Assert.Equal(RestaurantApplicationStatus.Pending, application.Status);
            Assert.False(application.Accepted);
        }

        [Fact]
        public void Applications_Can_Be_Accepted()
        {
            var application = new RestaurantApplication(Guid.NewGuid());
            application.Accept();

            Assert.Equal(RestaurantApplicationStatus.Accepted, application.Status);
            Assert.True(application.Accepted);
        }

        [Fact]
        public void Applications_Can_Not_Be_Accepted_If_The_Are_Already_Accepted()
        {
            var application = new RestaurantApplication(Guid.NewGuid());
            application.Accept();

            Assert.Throws<InvalidOperationException>(() => application.Accept());
        }
    }
}
