using System.Threading.Tasks;
using FoodSnap.Application.Users.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Users.Login
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
                return PresentError(result.Error);
            }

            return Ok();
        }
    }
}
