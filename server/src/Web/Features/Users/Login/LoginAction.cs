using System.Threading.Tasks;
using Web.Features.Users.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Users.Login
{
    public class LoginAction : Action
    {
        private readonly ISender sender;

        public LoginAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                return Error(result.Error);
            }

            return Ok();
        }
    }
}
