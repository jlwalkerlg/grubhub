﻿using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Services.DateTimeServices;

namespace Web.Features.Restaurants.ApproveRestaurant
{
    public class ApproveRestaurantHandler : IRequestHandler<ApproveRestaurantCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDateTimeProvider dateTimeProvider;

        public ApproveRestaurantHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            this.unitOfWork = unitOfWork;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(
            ApproveRestaurantCommand command,
            CancellationToken cancellationToken)
        {
            var id = new RestaurantId(command.RestaurantId);
            var restaurant = await unitOfWork.Restaurants.GetById(id);

            if (restaurant is null)
            {
                return Error.NotFound("Restaurant not found.");
            }

            restaurant.Approve();

            await unitOfWork.Publish(new RestaurantApprovedEvent(restaurant.Id, dateTimeProvider.UtcNow));

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
