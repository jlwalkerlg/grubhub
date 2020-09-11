using System;
using FoodSnap.Web.Queries.Users;

namespace FoodSnap.Web.Services.Authentication
{
    public interface IAuthenticator
    {
        bool IsAuthenticated { get; }
        Guid UserId { get; }
        void SignIn(UserDto user);
        void SignOut();
    }
}
