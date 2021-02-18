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
                Name = "Jordan",
                Email = "walker.jlg@gmail.com",
            };

            Insert(user);

            var request = new UpdateAuthUserDetailsCommand()
            {
                Name = "Bruno",
                Email = "bruno@gmail.com",
            };

            var response = await factory.GetAuthenticatedClient(user.Id).Put(
                "/auth/user",
                request);

            var found = UseTestDbContext(db => db.Users.Single());

            found.Name.ShouldBe(request.Name);
            found.Email.ShouldBe(request.Email);
        }
    }
}
