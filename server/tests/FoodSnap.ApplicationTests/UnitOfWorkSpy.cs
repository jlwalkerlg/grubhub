using System;
using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Events;
using FoodSnap.Application.Restaurants;
using FoodSnap.Application.Users;
using FoodSnap.ApplicationTests.Events;
using FoodSnap.ApplicationTests.Restaurants;
using FoodSnap.ApplicationTests.Users;

namespace FoodSnap.ApplicationTests
{
    public class UnitOfWorkSpy : IUnitOfWork
    {
        public IRestaurantRepository RestaurantRepository => RestaurantRepositorySpy;
        public RestaurantRepositorySpy RestaurantRepositorySpy { get; }

        public IRestaurantApplicationRepository RestaurantApplicationRepository => RestaurantApplicationRepositorySpy;
        public RestaurantApplicationRepositorySpy RestaurantApplicationRepositorySpy { get; }

        public IRestaurantManagerRepository RestaurantManagerRepository => RestaurantManagerRepositorySpy;
        public RestaurantManagerRepositorySpy RestaurantManagerRepositorySpy { get; }

        public IEventRepository EventRepository => EventRepositorySpy;
        public EventRepositorySpy EventRepositorySpy { get; }

        public bool Commited { get; private set; } = false;
        public Action OnCommit { get; set; }

        public UnitOfWorkSpy()
        {
            RestaurantRepositorySpy = new RestaurantRepositorySpy();
            RestaurantApplicationRepositorySpy = new RestaurantApplicationRepositorySpy();
            RestaurantManagerRepositorySpy = new RestaurantManagerRepositorySpy();
            EventRepositorySpy = new EventRepositorySpy();
        }

        public Task Commit()
        {
            Commited = true;

            if (OnCommit != null)
            {
                OnCommit();
            }

            return Task.CompletedTask;
        }
    }
}
