using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Domain;
using Web.Domain.Users;
using Web.Features.Users.RegisterCustomer;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Users.RegisterCustomer
{
    public class RegisterCustomerHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWork;
        private readonly HasherFake hasher;
        private readonly AuthenticatorSpy authenticator;
        private readonly RegisterCustomerHandler handler;

        public RegisterCustomerHandlerTests()
        {
            unitOfWork = new UnitOfWorkSpy();
            hasher = new HasherFake();
            authenticator = new AuthenticatorSpy();
            handler = new RegisterCustomerHandler(unitOfWork, hasher, authenticator);
        }

        [Fact]
        public async Task It_Registers_The_Customer()
        {
            var command = new RegisterCustomerCommand()
            {
                FirstName = "Jordan",
                LastName = "Walker",
                Email = "walker.jlg@gmail.com",
                Password = "password123",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            var customer = unitOfWork.UserRepositorySpy.Users.Single() as Customer;

            customer.FirstName.ShouldBe(command.FirstName);
            customer.LastName.ShouldBe(command.LastName);
            customer.Email.Address.ShouldBe(command.Email);
            customer.Password.ShouldBe(hasher.Hash(command.Password));

            authenticator.IsAuthenticated.ShouldBeTrue();
            authenticator.UserId.ShouldBe(customer.Id);
        }

        [Fact]
        public async Task It_Fails_If_The_Email_Is_Already_Taken()
        {
            var command = new RegisterCustomerCommand()
            {
                FirstName = "Jordan",
                LastName = "Walker",
                Email = "walker.jlg@gmail.com",
                Password = "password123",
            };

            var existingCustomer = new Customer(
                new UserId(Guid.NewGuid()),
                command.FirstName,
                command.LastName,
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
