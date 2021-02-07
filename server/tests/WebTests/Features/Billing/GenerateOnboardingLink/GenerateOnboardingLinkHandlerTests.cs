using System;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Domain;
using Web.Domain.Billing;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Billing.GenerateOnboardingLink;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Billing.GenerateOnboardingLink
{
    public class GenerateOnboardingLinkHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly BillingServiceSpy billingServiceSpy;
        private readonly GenerateOnboardingLinkHandler handler;

        public GenerateOnboardingLinkHandlerTests()
        {
            authenticatorSpy = new AuthenticatorSpy();

            unitOfWorkSpy = new UnitOfWorkSpy();

            billingServiceSpy = new BillingServiceSpy();

            handler = new GenerateOnboardingLinkHandler(
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
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2));

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.BillingAccounts.Add(billingAccount);

            authenticatorSpy.SignIn(Guid.NewGuid());

            var query = new GenerateOnboardingLinkQuery()
            {
                RestaurantId = restaurant.Id,
            };

            var result = await handler.Handle(query, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.Unauthorised);
        }
    }
}
