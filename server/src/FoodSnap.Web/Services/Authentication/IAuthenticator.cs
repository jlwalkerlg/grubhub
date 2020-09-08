using System;

namespace FoodSnap.Web.Services.Authentication
{
    public interface IAuthenticator
    {
        Guid? GetUserId();
    }
}
