using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Services.Antiforgery;

namespace Web.Features.Users.Register
{
    public class RegisterAction : Action
    {
        private readonly ISender sender;

        public RegisterAction(ISender sender)
        {
            this.sender = sender;
        }

        [IgnoreAntiforgeryValidation]
        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
