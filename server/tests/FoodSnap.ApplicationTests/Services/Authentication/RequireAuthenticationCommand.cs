using FoodSnap.Application;
using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.ApplicationTests.Services.Authentication
{
    [Authenticate]
    public class RequireAuthenticationCommand : IRequest
    {
    }
}
