using System.Threading.Tasks;
using FoodSnap.Application.Users.UpdateAuthUserDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsAction : Action
    {
        private readonly IMediator mediator;

        public UpdateAuthUserDetailsAction(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut("/auth/user")]
        public async Task<IActionResult> Execute([FromBody] UpdateAuthUserDetailsCommand command)
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
