using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Domain.Users;

namespace Web.Features.Restaurants.GenerateThumbnailUploadParams
{
    public class GenerateThumbnailUploadParamsAction : Action
    {
        private readonly ISender sender;

        public GenerateThumbnailUploadParamsAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpPost("/restaurant/thumbnail/generate-upload-params")]
        public async Task<IActionResult> Execute([FromBody] GenerateThumbnailUploadParamsCommand command)
        {
            var (response, error) = await sender.Send(command);

            return error ? Problem(error) : Ok(response);
        }
    }
}
