using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Events;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Application.Services.Hashing;
using FoodSnap.Application.Users;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;

namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantHandler : IRequestHandler<RegisterRestaurantCommand>
    {
        private readonly IHasher hasher;

        private readonly IRestaurantRepository restaurantRepository;
        private readonly IRestaurantManagerRepository restaurantManagerRepository;
        private readonly IEventRepository eventRepository;
        private readonly IUnitOfWork unitOfWork;

        private readonly IGeocoder geocoder;

        public RegisterRestaurantHandler(IHasher hasher, IUnitOfWork unitOfWork, IGeocoder geocoder)
        {
            this.hasher = hasher;

            restaurantRepository = unitOfWork.RestaurantRepository;
            restaurantManagerRepository = unitOfWork.RestaurantManagerRepository;
            eventRepository = unitOfWork.EventRepository;
            this.unitOfWork = unitOfWork;

            this.geocoder = geocoder;
        }

        public async Task<Result> Handle(RegisterRestaurantCommand command, CancellationToken cancellationToken)
        {
            var address = new Address(
                    command.AddressLine1,
                    command.AddressLine2,
                    command.Town,
                    new Postcode(command.Postcode));

            var coordinatesResult = await geocoder.GetCoordinates(address);

            if (!coordinatesResult.IsSuccess)
            {
                return coordinatesResult;
            }

            var coordinates = coordinatesResult.Value;

            var manager = new RestaurantManager(
                command.ManagerName,
                new Email(command.ManagerEmail),
                hasher.Hash(command.ManagerPassword));

            var restaurant = new Restaurant(
                manager.Id,
                command.RestaurantName,
                new PhoneNumber(command.RestaurantPhoneNumber),
                address,
                coordinates);

            var ev = new RestaurantRegisteredEvent(restaurant.Id, manager.Id);

            await restaurantManagerRepository.Add(manager);
            await restaurantRepository.Add(restaurant);
            await eventRepository.Add(ev);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
