using System;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Services;
using Web.Services.Geocoding;
using Web.Services.Hashing;
namespace Web.Features.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantHandler : IRequestHandler<RegisterRestaurantCommand>
    {
        private readonly IHasher hasher;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGeocoder geocoder;
        private readonly IClock clock;

        public RegisterRestaurantHandler(
            IHasher hasher,
            IUnitOfWork unitOfWork,
            IGeocoder geocoder,
            IClock clock)
        {
            this.hasher = hasher;
            this.unitOfWork = unitOfWork;
            this.geocoder = geocoder;
            this.clock = clock;
        }

        public async Task<Result> Handle(RegisterRestaurantCommand command, CancellationToken cancellationToken)
        {
            var geocodingResult = await geocoder.Geocode(command.Address);

            if (!geocodingResult)
            {
                return Error.BadRequest("Address is not a valid postal address.");
            }

            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                command.ManagerName,
                new Email(command.ManagerEmail),
                hasher.Hash(command.ManagerPassword));

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                command.RestaurantName,
                new PhoneNumber(command.RestaurantPhoneNumber),
                new Address(geocodingResult.Value.FormattedAddress),
                geocodingResult.Value.Coordinates);

            var menu = new Menu(restaurant.Id);

            var ev = new RestaurantRegisteredEvent(restaurant.Id, manager.Id, clock.UtcNow);

            await unitOfWork.RestaurantManagers.Add(manager);
            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Menus.Add(menu);
            await unitOfWork.Events.Add(ev);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
