using System;
using FoodSnap.Domain.Users;

namespace FoodSnap.Application.Services.Authentication
{
    public interface IAuthenticator
    {
        void SignIn(User user);
        void SignOut();
        bool IsAuthenticated { get; }
        Guid UserId { get; }
    }
}