using System;
using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Application.Services.Hashing;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantHandler : IRequestHandler<RegisterRestaurantCommand>
    {
        private readonly IHasher hasher;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGeocoder geocoder;

        public RegisterRestaurantHandler(IHasher hasher, IUnitOfWork unitOfWork, IGeocoder geocoder)
        {
            this.hasher = hasher;
            this.unitOfWork = unitOfWork;
            this.geocoder = geocoder;
        }

        public async Task<Result> Handle(RegisterRestaurantCommand command, CancellationToken cancellationToken)
        {
            var geocodingResult = await geocoder.Geocode(command.Address);

            if (!geocodingResult.IsSuccess)
            {
                return Result.Fail(Error.BadRequest("Address is not a valid postal address."));
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
                new Coordinates(geocodingResult.Value.Latitude, geocodingResult.Value.Longitude));

            var menu = new Menu(restaurant.Id);

            var ev = new RestaurantRegisteredEvent(restaurant.Id, manager.Id);

            await unitOfWork.RestaurantManagers.Add(manager);
            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Menus.Add(menu);
            await unitOfWork.Events.Add(ev);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
