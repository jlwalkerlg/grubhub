using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Web;
using Xunit;

namespace WebTests.Features.Billing.RefreshOnboarding
{
    public class RefreshOnboardingActionTests : ActionTestBase
    {
        private readonly AppSettings settings;

        public RefreshOnboardingActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
            settings = factory.Server.Services.GetRequiredService<AppSettings>();
        }

        [Fact]
        public async Task It_Redirects_To_The_Login_Page_If_Unauthenticated()
        {
            var uri = $"/stripe/onboarding/refresh?restaurant_id={Guid.NewGuid()}";
            var response = await GetClient().Get(uri);

            response.StatusCode.ShouldBe(302);
            response.Headers.Location?.OriginalString.ShouldBe(
                $"{settings.ClientUrl}/login?redirect_to={settings.ServerUrl}{uri}");
        }
    }
}
