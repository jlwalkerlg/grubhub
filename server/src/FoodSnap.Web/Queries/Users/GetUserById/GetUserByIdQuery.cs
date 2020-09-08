using System;
using FoodSnap.Application;
using FoodSnap.Web.Queries.Users;

namespace FoodSnap.Web.Queries.GetUserById
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
