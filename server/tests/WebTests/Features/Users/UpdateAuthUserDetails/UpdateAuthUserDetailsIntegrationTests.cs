using Shouldly;
using System.Linq;
using System.Threading.Tasks;
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
                FirstName = "Jordan",
                LastName = "Walker",
                Email = "walker.jlg@gmail.com",
            };

            Insert(user);

            var request = new UpdateAuthUserDetailsCommand()
            {
                FirstName = "Bruno",
                LastName = "Walker",
                Email = "bruno@gmail.com",
            };

            var response = await factory.GetAuthenticatedClient(user.Id).Put(
                "/auth/user",
                request);

            var found = UseTestDbContext(db => db.Users.Single());

            found.FirstName.ShouldBe(request.FirstName);
            found.LastName.ShouldBe(request.LastName);
            found.Email.ShouldBe(request.Email);
        }
    }
}
