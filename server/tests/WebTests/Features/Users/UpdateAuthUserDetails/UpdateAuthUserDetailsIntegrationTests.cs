using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Users.UpdateAuthUserDetails;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsIntegrationTests : IntegrationTestBase
    {
        public UpdateAuthUserDetailsIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Updates_The_Auth_Users_Details()
        {
            var user = new User()
            {
                Name = "Jordan",
                Email = "walker.jlg@gmail.com",
            };

            fixture.Insert(user);

            var request = new UpdateAuthUserDetailsCommand()
            {
                Name = "Bruno",
                Email = "bruno@gmail.com",
            };

            var response = await fixture.GetAuthenticatedClient(user.Id).Put(
                "/auth/user",
                request);

            var found = fixture.UseTestDbContext(db => db.Users.Single());

            found.Name.ShouldBe(request.Name);
            found.Email.ShouldBe(request.Email);
        }
    }
}
