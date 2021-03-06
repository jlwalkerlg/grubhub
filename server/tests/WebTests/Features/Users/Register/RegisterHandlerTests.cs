using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain.Users;
using Web.Features.Users.Register;
using WebTests.Doubles;
using Xunit;
using Shouldly;
using Web;
using Web.Domain;

namespace WebTests.Features.Users.Register
{
    public class RegisterHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWork;
        private readonly HasherFake hasher;
        private readonly AuthenticatorSpy authenticator;
        private readonly RegisterHandler handler;

        public RegisterHandlerTests()
        {
            unitOfWork = new UnitOfWorkSpy();
            hasher = new HasherFake();
            authenticator = new AuthenticatorSpy();
            handler = new RegisterHandler(unitOfWork, hasher, authenticator);
        }

        [Fact]
        public async Task It_Registers_The_Customer()
        {
            var command = new RegisterCommand()
            {
                Name = "Jordan Walker",
                Email = "walker.jlg@gmail.com",
                Password = "password123",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            var customer = unitOfWork.UserRepositorySpy.Users.Single() as Customer;

            customer.Name.ShouldBe(command.Name);
            customer.Email.Address.ShouldBe(command.Email);
            customer.Password.ShouldBe(hasher.Hash(command.Password));

            authenticator.IsAuthenticated.ShouldBeTrue();
            authenticator.UserId.ShouldBe(customer.Id);
        }

        [Fact]
        public async Task It_Fails_If_The_Email_Is_Already_Taken()
        {
            var command = new RegisterCommand()
            {
                Name = "Jordan Walker",
                Email = "walker.jlg@gmail.com",
                Password = "password123",
            };

            var existingCustomer = new Customer(
                new UserId(Guid.NewGuid()),
                command.Name,
                new Email(command.Email),
                hasher.Hash(command.Password));

            await unitOfWork.Users.Add(existingCustomer);

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.ValidationError);
            result.Errors.ShouldContainKey(nameof(command.Email));

            authenticator.IsAuthenticated.ShouldBeFalse();
        }
    }
}
