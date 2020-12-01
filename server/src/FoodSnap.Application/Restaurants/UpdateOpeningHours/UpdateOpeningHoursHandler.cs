using System;
using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.Application.Restaurants.UpdateOpeningHours
{
    public class UpdateOpeningHoursHandler : IRequestHandler<UpdateOpeningHoursCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;

        public UpdateOpeningHoursHandler(IUnitOfWork unitOfWork, IAuthenticator authenticator)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
        }

        public async Task<Result> Handle(UpdateOpeningHoursCommand command, CancellationToken cancellationToken)
        {
            var restaurant = await unitOfWork.Restaurants.GetById(new RestaurantId(command.RestaurantId));

            if (restaurant == null)
            {
                return Result.Fail(Error.NotFound("Restaurant not found."));
            }

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Result.Fail(Error.Unauthorised());
            }

            restaurant.OpeningTimes = new OpeningTimes()
            {
                Monday = command.MondayOpen == null ? null : new OpeningHours(TimeSpan.Parse(command.MondayOpen), TimeSpan.Parse(command.MondayClose)),
                Tuesday = command.TuesdayOpen == null ? null : new OpeningHours(TimeSpan.Parse(command.TuesdayOpen), TimeSpan.Parse(command.TuesdayClose)),
                Wednesday = command.WednesdayOpen == null ? null : new OpeningHours(TimeSpan.Parse(command.WednesdayOpen), TimeSpan.Parse(command.WednesdayClose)),
                Thursday = command.ThursdayOpen == null ? null : new OpeningHours(TimeSpan.Parse(command.ThursdayOpen), TimeSpan.Parse(command.ThursdayClose)),
                Friday = command.FridayOpen == null ? null : new OpeningHours(TimeSpan.Parse(command.FridayOpen), TimeSpan.Parse(command.FridayClose)),
                Saturday = command.SaturdayOpen == null ? null : new OpeningHours(TimeSpan.Parse(command.SaturdayOpen), TimeSpan.Parse(command.SaturdayClose)),
                Sunday = command.SundayOpen == null ? null : new OpeningHours(TimeSpan.Parse(command.SundayOpen), TimeSpan.Parse(command.SundayClose)),
            };

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
