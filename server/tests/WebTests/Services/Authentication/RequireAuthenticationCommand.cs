using Web;
using Web.Services.Authentication;

namespace WebTests.Services.Authentication
{
    [Authenticate]
    public class RequireAuthenticationCommand : IRequest
    {
    }
}
