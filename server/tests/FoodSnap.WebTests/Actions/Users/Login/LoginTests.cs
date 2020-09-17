using System.Threading.Tasks;
using FoodSnap.Application.Services.Hashing;
using FoodSnap.Domain;
using FoodSnap.Domain.Users;
using FoodSnap.WebTests.Functional;
using Xunit;

namespace FoodSnap.WebTests.Actions.Restaurants.Login
{
    public class LoginTests : FunctionalTestBase
    {
        private readonly IHasher hasher;

        public LoginTests(WebAppFactory factory) : base(factory)
        {
            hasher = GetService<IHasher>();
        }

        [Fact]
        public async Task It_Succeeds()
        {
            User user = new RestaurantManager(
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                hasher.Hash("password123"));

            await appDbContext.Users.AddAsync(user);
            await appDbContext.SaveChangesAsync();

            var response = await PostJson("/auth/login", new
            {
                email = "walker.jlg@gmail.com",
                password = "password123",
            });

            response.EnsureSuccessStatusCode();
        }
    }
}
