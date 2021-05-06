using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Services.Authentication;
using Web.Services.DateTimeServices;

namespace Web.Features.Restaurants.UpdateOpeningHours
{
    public class UpdateOpeningHoursHandler : IRequestHandler<UpdateOpeningHoursCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IDateTimeProvider dateTimeProvider;

        public UpdateOpeningHoursHandler(
            IUnitOfWork unitOfWork,
            IAuthenticator authenticator,
            IDateTimeProvider dateTimeProvider)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(UpdateOpeningHoursCommand command, CancellationToken cancellationToken)
        {
            var restaurant = await unitOfWork.Restaurants.GetById(new RestaurantId(command.RestaurantId));

            if (restaurant == null)
            {
                return Error.NotFound("Restaurant not found.");
            }

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Error.Unauthorised();
            }

            restaurant.OpeningTimes = new OpeningTimes()
            {
                Monday = OpeningHours.Parse(command.MondayOpen, command.MondayClose),
                Tuesday = OpeningHours.Parse(command.TuesdayOpen, command.TuesdayClose),
                Wednesday = OpeningHours.Parse(command.WednesdayOpen, command.WednesdayClose),
                Thursday = OpeningHours.Parse(command.ThursdayOpen, command.ThursdayClose),
                Friday = OpeningHours.Parse(command.FridayOpen, command.FridayClose),
                Saturday = OpeningHours.Parse(command.SaturdayOpen, command.SaturdayClose),
                Sunday = OpeningHours.Parse(command.SundayOpen, command.SundayClose),
            };

            await unitOfWork.Publish(new RestaurantUpdatedEvent(restaurant.Id, dateTimeProvider.UtcNow));
            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
