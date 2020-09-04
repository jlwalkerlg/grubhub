using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Events;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Application.Users;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;

namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantHandler : IRequestHandler<RegisterRestaurantCommand>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IRestaurantApplicationRepository restaurantApplicationRepository;
        private readonly IRestaurantManagerRepository restaurantManagerRepository;
        private readonly IEventRepository eventRepository;
        private readonly IUnitOfWork unitOfWork;

        private readonly IGeocoder geocoder;

        public RegisterRestaurantHandler(IUnitOfWork unitOfWork, IGeocoder geocoder)
        {
            restaurantRepository = unitOfWork.RestaurantRepository;
            restaurantApplicationRepository = unitOfWork.RestaurantApplicationRepository;
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

            var restaurant = new Restaurant(
                command.RestaurantName,
                new PhoneNumber(command.RestaurantPhoneNumber),
                address,
                coordinates);

            var application = new RestaurantApplication(restaurant.Id);

            // TODO: hash password
            var manager = new RestaurantManager(
                command.ManagerName,
                new Email(command.ManagerEmail),
                command.ManagerPassword,
                restaurant.Id);

            var ev = new RestaurantRegisteredEvent(restaurant.Id, manager.Id);

            await restaurantRepository.Add(restaurant);
            await restaurantApplicationRepository.Add(application);
            await restaurantManagerRepository.Add(manager);
            await eventRepository.Add(ev);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
