using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Domain;
using Web.Domain.Billing;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Billing.SetupBilling;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Billing.SetupBilling
{
    public class SetupBillingHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly BillingServiceSpy billingServiceSpy;
        private readonly SetupBillingHandler handler;

        public SetupBillingHandlerTests()
        {
            authenticatorSpy = new AuthenticatorSpy();

            unitOfWorkSpy = new UnitOfWorkSpy();

            billingServiceSpy = new BillingServiceSpy();

            handler = new SetupBillingHandler(
                authenticatorSpy,
                unitOfWorkSpy,
                billingServiceSpy);
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Unauthorised()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new PhoneNumber("01234567890"),
                new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(54, -2));

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.BillingAccounts.Add(billingAccount);

            await authenticatorSpy.SignIn(Guid.NewGuid());

            var command = new SetupBillingCommand()
            {
                RestaurantId = restaurant.Id,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.Unauthorised);
        }

        [Fact]
        public async Task It_Sets_Up_A_New_Account_If_No_Billing_Account_Exists()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new PhoneNumber("01234567890"),
                new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(54, -2));

            await unitOfWorkSpy.Restaurants.Add(restaurant);

            await authenticatorSpy.SignIn(restaurant.ManagerId);

            billingServiceSpy.AccountId = Guid.NewGuid().ToString();
            billingServiceSpy.OnboardingLink = Guid.NewGuid().ToString();

            var command = new SetupBillingCommand()
            {
                RestaurantId = restaurant.Id,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();
            result.Value.ShouldBe(billingServiceSpy.OnboardingLink);

            var accounts = unitOfWorkSpy.BillingAccountsRepositorySpy.Accounts;

            accounts.ShouldHaveSingleItem();

            var account = accounts.Single();

            account.RestaurantId.ShouldBe(restaurant.Id);
            account.Enabled.ShouldBe(false);
        }
    }
}
