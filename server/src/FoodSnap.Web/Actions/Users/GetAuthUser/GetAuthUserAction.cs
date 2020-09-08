using System.Threading.Tasks;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Queries.GetUserById;
using FoodSnap.Web.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Users.GetAuthUser
{
    public class GetAuthUserAction : Action
    {
        private readonly IMediator mediator;
        private readonly IAuthenticator authenticator;

        public GetAuthUserAction(
            IMediator mediator,
            IAuthenticator authenticator)
        {
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        [HttpGet("/auth/user")]
        public async Task<IActionResult> GetAuthUser()
        {
            var id = authenticator.GetUserId();

            if (id is null)
            {
                return StatusCode(403);
            }

            var query = new GetUserByIdQuery(id.Value);

            var user = (await mediator.Send(query)).Value;

            if (user == null)
            {
                return StatusCode(403);
            }

            return Ok(new DataEnvelope(user));
        }
    }
}
