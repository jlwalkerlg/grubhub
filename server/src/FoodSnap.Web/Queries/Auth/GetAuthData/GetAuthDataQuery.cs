using System;
using FoodSnap.Application;

namespace FoodSnap.Web.Queries.Auth.GetAuthData
{
    public class GetAuthDataQuery : IRequest<AuthDataDto>
    {
        public GetAuthDataQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
