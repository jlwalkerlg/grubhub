using System.Threading.Tasks;
using FoodSnap.Application.Services;
using FoodSnap.Application.Users;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;

namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantHandler : IRequestHandler<RegisterRestaurantCommand, Result>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IRestaurantManagerRepository restaurantManagerRepository;
        private readonly IEventRepository eventRepository;
        private readonly IUnitOfWork unitOfWork;

        private readonly IGeocoder geocoder;

        public RegisterRestaurantHandler(IUnitOfWork unitOfWork, IGeocoder geocoder)
        {
            restaurantRepository = unitOfWork.RestaurantRepository;
            restaurantManagerRepository = unitOfWork.RestaurantManagerRepository;
            eventRepository = unitOfWork.EventRepository;
            this.unitOfWork = unitOfWork;

            this.geocoder = geocoder;
        }

        public async Task<Result> Handle(RegisterRestaurantCommand command)
        {
            var address = new Address(
                    command.AddressLine1,
                    command.AddressLine2,
                    command.Town,
                    new Postcode(command.Postcode));

            var coordinates = await geocoder.GetCoordinates(address);

            var restaurant = new Restaurant(
                command.RestaurantName,
                new PhoneNumber(command.RestaurantPhoneNumber),
                address,
                coordinates
            );

            await restaurantRepository.Add(restaurant);

            var manager = new RestaurantManager(
                command.ManagerName,
                new Email(command.ManagerEmail),
                command.ManagerPassword,
                restaurant.Id
            );

            await restaurantManagerRepository.Add(manager);

            var ev = new RestaurantRegisteredEvent(restaurant.Id, manager.Id);

            await eventRepository.Add(ev);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
