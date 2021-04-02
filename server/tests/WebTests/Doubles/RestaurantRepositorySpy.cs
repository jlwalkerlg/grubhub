using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Features.Restaurants;

namespace WebTests.Doubles
{
    public class RestaurantRepositorySpy : IRestaurantRepository
    {
        public List<Restaurant> Restaurants { get; } = new();

        public Task Add(Restaurant restaurant)
        {
            Restaurants.Add(restaurant);
            return Task.CompletedTask;
        }

        public Task<Restaurant> GetById(Guid id)
        {
            return Task.FromResult(Restaurants.FirstOrDefault(x => x.Id.Value == id));
        }

        public Task<Restaurant> GetByManagerId(Guid managerId)
        {
            return Task.FromResult(Restaurants.SingleOrDefault(x => x.ManagerId.Value == managerId));
        }
    }
}
