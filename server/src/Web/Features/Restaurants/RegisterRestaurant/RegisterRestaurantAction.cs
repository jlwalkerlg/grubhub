using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Web.Services.Antiforgery;

namespace Web.Features.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantAction : Action
    {
        private readonly ISender sender;
        private readonly IAntiforgery antiforgery;

        public RegisterRestaurantAction(ISender sender, IAntiforgery antiforgery)
        {
            this.sender = sender;
            this.antiforgery = antiforgery;
        }

        [IgnoreAntiforgeryValidation]
        [HttpPost("/restaurants/register")]
        public async Task<IActionResult> Execute(RegisterRestaurantCommand command)
        {
            var result = await sender.Send(command);

            if (!result) return Problem(result.Error);

            var token = antiforgery.GetAndStoreTokens(HttpContext);

            return StatusCode(201, new RegisterRestaurantResponse()
            {
                XsrfToken = token.RequestToken,
            });
        }
    }
}
