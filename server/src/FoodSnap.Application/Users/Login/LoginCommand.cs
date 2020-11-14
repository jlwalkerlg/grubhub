﻿namespace FoodSnap.Application.Users.Login
{
    public record LoginCommand : IRequest
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
