using System;

namespace FoodSnap.Domain.Users
{
    public class UserId : GuidId
    {
        public UserId(Guid value) : base(value)
        {
        }
    }
}
