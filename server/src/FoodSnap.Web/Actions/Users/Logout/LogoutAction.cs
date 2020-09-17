using System.Threading.Tasks;
using FoodSnap.Application.Users.Logout;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Users.Logout
{
    public class LogoutAction : Action
    {
        private readonly IMediator mediator;

        public LogoutAction(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("/auth/logout")]
        public async Task<IActionResult> Execute()
        {
            var result = await mediator.Send(new LogoutCommand());

            return Ok();
        }
    }
}
