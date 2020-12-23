using System;

namespace Application.Services.Authentication
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthenticateAttribute : Attribute
    {
    }
}
