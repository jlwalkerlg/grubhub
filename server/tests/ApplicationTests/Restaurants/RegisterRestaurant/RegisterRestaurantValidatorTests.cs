using System;
using System.Threading.Tasks;
using Web.Features.Restaurants.RegisterRestaurant;
using Web.Domain;
using Web.Domain.Users;
using Xunit;

namespace ApplicationTests.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantValidatorTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly RegisterRestaurantValidator validator;

        public RegisterRestaurantValidatorTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();

            validator = new RegisterRestaurantValidator(unitOfWorkSpy);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Manager_Names(string name)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetManagerName(name)
                .Build();

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.ManagerName)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("blahblahblah")]
        public async Task Disallows_Invalid_Manager_Emails(string email)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetManagerEmail(email)
                .Build();

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.ManagerEmail)));
        }

        [Fact]
        public async Task Disallows_Emails_That_Are_Already_Taken()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Mr Wong",
                new Email("wong@test.com"),
                "password123");

            await unitOfWorkSpy.RestaurantManagers.Add(manager);

            var command = new RegisterRestaurantCommandBuilder()
                .SetManagerEmail(manager.Email.Address)
                .Build();

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.ManagerEmail)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1234567")]
        public async Task Disallows_Invalid_Manager_Passwords(string password)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetManagerPassword(password)
                .Build();

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.ManagerPassword)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Restaurant_Names(string name)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetRestaurantName(name)
                .Build();

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.RestaurantName)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1352314")]
        public async Task Disallows_Invalid_Restaurant_Phone_Numbers(string number)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetRestaurantPhoneNumber(number)
                .Build();

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.RestaurantPhoneNumber)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Disallows_Invalid_Restaurant_Addresses(string address)
        {
            var command = new RegisterRestaurantCommandBuilder()
                .SetAddress(address)
                .Build();

            var result = await validator.Validate(command);

            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Errors.ContainsKey(nameof(command.Address)));
        }
    }
}
