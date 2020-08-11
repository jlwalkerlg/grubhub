using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;

namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantHandler : IRequestHandler<RegisterRestaurantCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IRestaurantManagerRepository restaurantManagerRepository;
        private readonly IEventRepository eventRepository;

        public RegisterRestaurantHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            restaurantRepository = unitOfWork.RestaurantRepository;
            restaurantManagerRepository = unitOfWork.RestaurantManagerRepository;
            eventRepository = unitOfWork.EventRepository;
        }

        public async Task<Result> Handle(RegisterRestaurantCommand command)
        {
            var restaurant = new Restaurant(
                command.RestaurantName,
                new PhoneNumber(command.RestaurantPhoneNumber),
                new Address(
                    command.AddressLine1,
                    command.AddressLine2,
                    command.Town,
                    new Postcode(command.Postcode)
                )
            );

            await restaurantRepository.Add(restaurant);

            var manager = new RestaurantManager(
                command.ManagerName,
                new Email(command.ManagerEmail),
                command.ManagerPassword,
                restaurant.Id
            );

            await restaurantManagerRepository.Add(manager);

            var ev = new RestaurantRegisteredEvent();

            await eventRepository.Add(ev);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
