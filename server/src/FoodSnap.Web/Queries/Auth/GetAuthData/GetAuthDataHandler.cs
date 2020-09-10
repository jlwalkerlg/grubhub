using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FoodSnap.Application;
using FoodSnap.Infrastructure.Persistence;
using FoodSnap.Web.Queries.Restaurants;
using FoodSnap.Web.Queries.Users;

namespace FoodSnap.Web.Queries.Auth.GetAuthData
{
    public class GetAuthDataHandler : IRequestHandler<GetAuthDataQuery, AuthDataDto>
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public GetAuthDataHandler(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Result<AuthDataDto>> Handle(GetAuthDataQuery request, CancellationToken cancellationToken)
        {
            var sql = @"
                SELECT
                    ""Users"".""Id"" AS ""UserId"",
                    ""Users"".""Name"" AS ""UserName"",
                    ""Users"".""Email"",
                    ""Users"".""Password"",
                    ""Users"".""UserType"",
                    ""Restaurants"".""Id"" AS ""RestaurantId"",
                    ""Restaurants"".""Name"" AS ""RestaurantName"",
                    ""Restaurants"".""PhoneNumber"",
                    ""Restaurants"".""AddressLine1"",
                    ""Restaurants"".""AddressLine2"",
                    ""Restaurants"".""Town"",
                    ""Restaurants"".""Postcode"",
                    ""Restaurants"".""Latitude"",
                    ""Restaurants"".""Longitude"",
                    ""Restaurants"".""Status""
                FROM
                    ""Users""
                LEFT JOIN ""Restaurants"" ON ""Users"".""Id"" = ""Restaurants"".""ManagerId""
                WHERE
                    ""Users"".""Email"" = @Email";
            ;

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var row = await connection.QueryFirstOrDefaultAsync<Row>(sql, new { Email = "walker.jlg@gmail.com" });

                if (row == null)
                {
                    return Result.Ok<AuthDataDto>(null);
                }

                var user = new UserDto
                {
                    Id = row.UserId,
                    Name = row.UserName,
                    Email = row.Email,
                    Password = row.Password,
                    Role = row.UserType,
                };

                RestaurantDto restaurant = null;

                if (row.RestaurantId != null)
                {
                    restaurant = new RestaurantDto
                    {
                        Id = row.RestaurantId.Value,
                        ManagerId = row.UserId,
                        Name = row.RestaurantName,
                        PhoneNumber = row.PhoneNumber,
                        AddressLine1 = row.AddressLine1,
                        AddressLine2 = row.AddressLine2,
                        Town = row.Town,
                        Postcode = row.Postcode,
                        Latitude = row.Latitude,
                        Longitude = row.Longitude,
                        Status = row.Status,
                    };
                }

                return Result.Ok(new AuthDataDto
                {
                    User = user,
                    Restaurant = restaurant,
                });
            }
        }

        private class Row
        {
            public Guid UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserType { get; set; }
            public Guid? RestaurantId { get; set; }
            public string RestaurantName { get; set; }
            public string PhoneNumber { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string Town { get; set; }
            public string Postcode { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
            public string Status { get; set; }
        }
    }
}
