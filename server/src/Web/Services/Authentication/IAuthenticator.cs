using System.Threading.Tasks;
using Web.Domain.Users;

namespace Web.Services.Authentication
{
    public interface IAuthenticator
    {
        Task SignIn(User user);
        Task SignOut();
        bool IsAuthenticated { get; }
        UserId UserId { get; }
    }
}
