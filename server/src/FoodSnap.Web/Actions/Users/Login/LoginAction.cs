using System.Threading.Tasks;
using FoodSnap.Application.Users.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Users.Login
{
    public class LoginAction : Action
    {
        private readonly IMediator mediator;

        public LoginAction(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await mediator.Send(command);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            return Ok();
        }
    }
}
