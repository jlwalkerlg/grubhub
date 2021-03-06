using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Web.Domain.Users;
using Web.Features.Restaurants.UpdateOpeningHours;

namespace Web.Features.Restaurants.UpdateOpeningTimes
{
    public class UpdateOpeningTimesAction : Action
    {
        private readonly ISender sender;

        public UpdateOpeningTimesAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpPut("/restaurants/{restaurantId:guid}/opening-times")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromBody] UpdateOpeningTimesRequest request)
        {
            var command = new UpdateOpeningHoursCommand()
            {
                RestaurantId = restaurantId,
                MondayOpen = string.IsNullOrWhiteSpace(request.MondayOpen)
                    ? null : request.MondayOpen,
                MondayClose = string.IsNullOrWhiteSpace(request.MondayClose)
                    ? null : request.MondayClose,
                TuesdayOpen = string.IsNullOrWhiteSpace(request.TuesdayOpen)
                    ? null : request.TuesdayOpen,
                TuesdayClose = string.IsNullOrWhiteSpace(request.TuesdayClose)
                    ? null : request.TuesdayClose,
                WednesdayOpen = string.IsNullOrWhiteSpace(request.WednesdayOpen)
                    ? null : request.WednesdayOpen,
                WednesdayClose = string.IsNullOrWhiteSpace(request.WednesdayClose)
                    ? null : request.WednesdayClose,
                ThursdayOpen = string.IsNullOrWhiteSpace(request.ThursdayOpen)
                    ? null : request.ThursdayOpen,
                ThursdayClose = string.IsNullOrWhiteSpace(request.ThursdayClose)
                    ? null : request.ThursdayClose,
                FridayOpen = string.IsNullOrWhiteSpace(request.FridayOpen)
                    ? null : request.FridayOpen,
                FridayClose = string.IsNullOrWhiteSpace(request.FridayClose)
                    ? null : request.FridayClose,
                SaturdayOpen = string.IsNullOrWhiteSpace(request.SaturdayOpen)
                    ? null : request.SaturdayOpen,
                SaturdayClose = string.IsNullOrWhiteSpace(request.SaturdayClose)
                    ? null : request.SaturdayClose,
                SundayOpen = string.IsNullOrWhiteSpace(request.SundayOpen)
                    ? null : request.SundayOpen,
                SundayClose = string.IsNullOrWhiteSpace(request.SundayClose)
                    ? null : request.SundayClose,
            };

            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
