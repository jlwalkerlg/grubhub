using FoodSnap.Application;

namespace FoodSnap.Web.Queries.Auth.GetAuthData
{
    public class GetAuthDataQuery : IRequest<AuthDataDto>
    {
        public GetAuthDataQuery(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}
