using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Domain.Users;

namespace Web.Features.Restaurants.ConfirmThumbnailUpload
{
    public class ConfirmThumbnailUploadAction : Action
    {
        private readonly ISender sender;

        public ConfirmThumbnailUploadAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpPut("/restaurant/thumbnail/confirm")]
        public async Task<IActionResult> Execute([FromBody] ConfirmThumbnailUploadCommand command)
        {
            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
