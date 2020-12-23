using Application;
using Application.Services.Authentication;

namespace ApplicationTests.Services.Authentication
{
    [Authenticate]
    public class RequireAuthenticationCommand : IRequest
    {
    }
}
