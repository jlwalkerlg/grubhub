using Shouldly;
using Web.Domain;
using Xunit;

namespace WebTests.Domain
{
    public class PostcodeTests
    {
        [Theory]
        [InlineData("M1 1AA")]
        [InlineData("M60 1NW")]
        [InlineData("CR2 6XH")]
        [InlineData("DN55 1PT")]
        [InlineData("W1A 1HQ")]
        [InlineData("EC1A 1BB")]
        public void Postcode_Validation_Passes(string postcode)
        {
            Postcode.IsValid(postcode).ShouldBeTrue();
        }
    }
}
