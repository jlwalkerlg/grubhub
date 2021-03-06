using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Services.Geocoding;
using Web.Services.Hashing;

namespace Web.Features.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantHandler : IRequestHandler<RegisterRestaurantCommand>
    {
        private readonly IHasher hasher;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGeocoder geocoder;

        public RegisterRestaurantHandler(
            IHasher hasher,
            IUnitOfWork unitOfWork,
            IGeocoder geocoder)
        {
            this.hasher = hasher;
            this.unitOfWork = unitOfWork;
            this.geocoder = geocoder;
        }

        public async Task<Result> Handle(RegisterRestaurantCommand command, CancellationToken cancellationToken)
        {
            if (await unitOfWork.Users.EmailExists(command.ManagerEmail))
            {
                return Error.ValidationError(new Dictionary<string, string>()
                {
                    { nameof(command.ManagerEmail), "Email already taken." },
                });
            }

            var (coordinates, geocodingError) = await geocoder.LookupCoordinates(command.Postcode);

            if (geocodingError)
            {
                return Error.BadRequest("Address was not recognised.");
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
                new Address(
                    command.AddressLine1,
                    command.AddressLine2,
                    command.City,
                    new Postcode(command.Postcode)),
                coordinates);

            await unitOfWork.Users.Add(manager);
            await unitOfWork.Restaurants.Add(restaurant);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
