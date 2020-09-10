using System;
using FoodSnap.Web.Queries.Users;

namespace FoodSnap.Web.Services.Authentication
{
    public interface IAuthenticator
    {
        Guid? GetUserId();
        void SignIn(UserDto user);
        void SignOut();
    }
}
