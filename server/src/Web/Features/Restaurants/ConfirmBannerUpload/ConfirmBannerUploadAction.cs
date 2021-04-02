using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Domain.Users;

namespace Web.Features.Restaurants.ConfirmBannerUpload
{
    public class ConfirmBannerUploadAction : Action
    {
        private readonly ISender sender;

        public ConfirmBannerUploadAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpPut("/restaurant/banner/confirm")]
        public async Task<IActionResult> Execute([FromBody] ConfirmBannerUploadCommand command)
        {
            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
