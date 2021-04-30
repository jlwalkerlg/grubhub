using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Web.Services.Antiforgery;

namespace Web.Features.Users.RegisterCustomer
{
    public class RegisterCustomerAction : Action
    {
        private readonly ISender sender;
        private readonly IAntiforgery antiforgery;

        public RegisterCustomerAction(ISender sender, IAntiforgery antiforgery)
        {
            this.sender = sender;
            this.antiforgery = antiforgery;
        }

        [IgnoreAntiforgeryValidation]
        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerCommand command)
        {
            var result = await sender.Send(command);

            if (!result) return Problem(result.Error);

            var token = antiforgery.GetAndStoreTokens(HttpContext);

            return Ok(new RegisterCustomerResponse()
            {
                XsrfToken = token.RequestToken,
            });
        }
    }
}
