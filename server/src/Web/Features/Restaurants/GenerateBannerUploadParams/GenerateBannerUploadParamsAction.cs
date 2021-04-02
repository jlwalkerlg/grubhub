using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Domain.Users;

namespace Web.Features.Restaurants.GenerateBannerUploadParams
{
    public class GenerateBannerUploadParamsAction : Action
    {
        private readonly ISender sender;

        public GenerateBannerUploadParamsAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpPost("/restaurant/banner/generate-upload-params")]
        public async Task<IActionResult> Execute([FromBody] GenerateBannerUploadParamsCommand command)
        {
            var (response, error) = await sender.Send(command);

            return error ? Problem(error) : Ok(response);
        }
    }
}
