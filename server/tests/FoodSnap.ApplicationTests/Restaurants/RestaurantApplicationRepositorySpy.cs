using System.Collections.Generic;
using System.Threading.Tasks;
using FoodSnap.Application.Restaurants;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.ApplicationTests.Restaurants
{
    public class RestaurantApplicationRepositorySpy : IRestaurantApplicationRepository
    {
        public List<RestaurantApplication> Applications { get; } = new List<RestaurantApplication>();

        public Task Add(RestaurantApplication application)
        {
            Applications.Add(application);
            return Task.CompletedTask;
        }
    }
}
