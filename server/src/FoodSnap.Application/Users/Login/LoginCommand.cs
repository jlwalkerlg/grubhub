namespace FoodSnap.Application.Users.Login
{
    public class LoginCommand : IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
