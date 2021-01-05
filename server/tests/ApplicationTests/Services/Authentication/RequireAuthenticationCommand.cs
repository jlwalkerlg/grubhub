using Web;
using Web.Services.Authentication;

namespace ApplicationTests.Services.Authentication
{
    [Authenticate]
    public class RequireAuthenticationCommand : IRequest
    {
    }
}
