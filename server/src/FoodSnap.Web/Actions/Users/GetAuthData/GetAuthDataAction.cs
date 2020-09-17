using System.Threading.Tasks;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Queries.Auth.GetAuthData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Users.GetAuthData
{
    public class GetAuthDataAction : Action
    {
        private readonly IMediator mediator;
        private readonly IAuthenticator authenticator;

        public GetAuthDataAction(IMediator mediator, IAuthenticator authenticator)
        {
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        [HttpGet("/auth/data")]
        public async Task<IActionResult> Execute()
        {
            if (!authenticator.IsAuthenticated)
            {
                return StatusCode(403);
            }

            var id = authenticator.UserId;

            var data = (await mediator.Send(new GetAuthDataQuery(id))).Value;

            return Ok(new DataEnvelope
            {
                Data = data
            });
        }
    }
}
