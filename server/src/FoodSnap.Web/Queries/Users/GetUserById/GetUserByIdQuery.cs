using System;
using FoodSnap.Application;

namespace FoodSnap.Web.Queries.Users.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
