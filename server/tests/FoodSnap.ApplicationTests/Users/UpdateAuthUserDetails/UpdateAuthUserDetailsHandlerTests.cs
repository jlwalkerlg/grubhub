using System;
using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Users.UpdateAuthUserDetails;
using FoodSnap.ApplicationTests.Services.Authentication;
using FoodSnap.Domain;
using FoodSnap.Domain.Users;
using Xunit;

namespace FoodSnap.ApplicationTests.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UpdateAuthUserDetailsHandler handler;

        public UpdateAuthUserDetailsHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();
            authenticatorSpy = new AuthenticatorSpy();

            handler = new UpdateAuthUserDetailsHandler(authenticatorSpy, unitOfWorkSpy);
        }

        [Fact]
        public async Task It_Updates_The_Authenticated_User()
        {
            User authUser = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");
            authenticatorSpy.User = authUser;
            await unitOfWorkSpy.UserRepositorySpy.Add(authUser);

            var command = new UpdateAuthUserDetailsCommand
            {
                Name = "New Name",
                Email = "new@email.com"
            };

            var result = await handler.Handle(command, default(CancellationToken));

            Assert.True(result.IsSuccess);

            Assert.Equal(command.Name, authUser.Name);
            Assert.Equal(command.Email, authUser.Email.Address);

            Assert.True(unitOfWorkSpy.Commited);
        }
    }
}
