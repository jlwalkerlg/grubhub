using System;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Domain;
using Web.Domain.Billing;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Billing.GetOnboardingLink;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Billing.GetOnboardingLink
{
    public class GetOnboardingLinkHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly GetOnboardingLinkHandler handler;

        public GetOnboardingLinkHandlerTests()
        {
            authenticatorSpy = new AuthenticatorSpy();

            unitOfWorkSpy = new UnitOfWorkSpy();

            handler = new GetOnboardingLinkHandler(
                authenticatorSpy,
                unitOfWorkSpy,
                new BillingServiceSpy());
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

            var query = new GetOnboardingLinkQuery()
            {
                RestaurantId = restaurant.Id,
            };

            var result = await handler.Handle(query, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.Unauthorised);
        }
    }
}
