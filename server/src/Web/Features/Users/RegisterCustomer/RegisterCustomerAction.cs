using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Services.Antiforgery;

namespace Web.Features.Users.RegisterCustomer
{
    public class RegisterCustomerAction : Action
    {
        private readonly ISender sender;

        public RegisterCustomerAction(ISender sender)
        {
            this.sender = sender;
        }

        [IgnoreAntiforgeryValidation]
        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerCommand command)
        {
            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
