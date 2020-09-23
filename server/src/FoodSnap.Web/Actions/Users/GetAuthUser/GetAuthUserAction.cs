using System.Threading.Tasks;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Queries.Users.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Users.GetAuthUser
{
    public class GetAuthUserAction : Action
    {
        private readonly IMediator mediator;
        private readonly IAuthenticator authenticator;

        public GetAuthUserAction(IMediator mediator, IAuthenticator authenticator)
        {
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        [HttpGet("/auth/user")]
        public async Task<IActionResult> Execute()
        {
            if (!authenticator.IsAuthenticated)
            {
                return StatusCode(401);
            }

            var id = authenticator.UserId;

            var result = await mediator.Send(new GetUserByIdQuery(id));

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            return Ok(new DataEnvelope
            {
                Data = result.Value
            });
        }
    }
}
