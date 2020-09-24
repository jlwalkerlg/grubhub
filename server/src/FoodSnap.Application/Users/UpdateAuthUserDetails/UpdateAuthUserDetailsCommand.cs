using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Users.UpdateAuthUserDetails
{
    [Authenticate]
    public class UpdateAuthUserDetailsCommand : IRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
