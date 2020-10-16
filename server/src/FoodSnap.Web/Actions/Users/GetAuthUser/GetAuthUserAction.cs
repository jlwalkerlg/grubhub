using System.Threading.Tasks;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Actions.Users.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Users.GetAuthUser
{
    public class GetAuthUserAction : Action
    {
        private readonly ISender sender;
        private readonly IAuthenticator authenticator;

        public GetAuthUserAction(ISender sender, IAuthenticator authenticator)
        {
            this.sender = sender;
            this.authenticator = authenticator;
        }

        [HttpGet("/auth/user")]
        public async Task<IActionResult> Execute()
        {
            if (!authenticator.IsAuthenticated)
            {
                return StatusCode(401);
            }

            var result = await sender.Send(
                new GetUserByIdQuery(authenticator.UserId.Value));

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            return Ok(new DataEnvelope<UserDto>
            {
                Data = result.Value
            });
        }
    }
}
