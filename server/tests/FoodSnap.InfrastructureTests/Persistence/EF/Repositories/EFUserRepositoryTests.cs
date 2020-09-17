using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Users;
using FoodSnap.Infrastructure.Persistence.EF.Repositories;
using Xunit;

namespace FoodSnap.InfrastructureTests.Persistence.EF.Repositories
{
    public class EFUserRepositoryTests : EFRepositoryTestBase
    {
        private readonly EFUserRepository repository;

        public EFUserRepositoryTests(EFContextFixture fixture) : base(fixture)
        {
            repository = new EFUserRepository(context);
        }

        [Fact]
        public async Task It_Gets_A_User_By_Email()
        {
            var user = new RestaurantManager(
                "Mr Chow",
                new Email("mr@chow.com"),
                "wongkarwai");

            await context.Users.AddAsync(user);
            FlushContext();

            var found = await repository.GetByEmail("mr@chow.com");

            Assert.Equal(user.Id, found.Id);
            Assert.Equal(user.Name, found.Name);
            Assert.Equal(user.Email, found.Email);
            Assert.Equal(user.Password, found.Password);
        }
    }
}
