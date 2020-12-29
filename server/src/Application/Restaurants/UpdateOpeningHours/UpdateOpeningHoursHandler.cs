using System.Threading;
using System.Threading.Tasks;
using Application.Services.Authentication;
using Domain.Restaurants;

namespace Application.Restaurants.UpdateOpeningHours
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
                Monday = OpeningHours.Parse(command.MondayOpen, command.MondayClose),
                Tuesday = OpeningHours.Parse(command.TuesdayOpen, command.TuesdayClose),
                Wednesday = OpeningHours.Parse(command.WednesdayOpen, command.WednesdayClose),
                Thursday = OpeningHours.Parse(command.ThursdayOpen, command.ThursdayClose),
                Friday = OpeningHours.Parse(command.FridayOpen, command.FridayClose),
                Saturday = OpeningHours.Parse(command.SaturdayOpen, command.SaturdayClose),
                Sunday = OpeningHours.Parse(command.SundayOpen, command.SundayClose),
            };

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
