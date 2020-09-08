using FoodSnap.Application;
using FoodSnap.Web.Actions.Users;

namespace FoodSnap.Web.Queries.Users.GetUserByEmail
{
    public class GetUserByEmailQuery : IRequest<UserDto>
    {
        public string Email { get; set; }

        public GetUserByEmailQuery(string email)
        {
            Email = email;
        }
    }
}
