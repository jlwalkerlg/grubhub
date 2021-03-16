using Shouldly;
using Web.Domain;
using Xunit;

namespace WebTests.Domain
{
    public class MobileNumberTests
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("526789102", false)]
        [InlineData("071", false)]
        [InlineData("+4471", false)]
        [InlineData("07123456789", true)]
        [InlineData("07 123 456789", true)]
        [InlineData("+447123456789", true)]
        [InlineData("+447 123 456789", true)]
        public void IsValid(string number, bool valid)
        {
            MobileNumber.IsValid(number).ShouldBe(valid);
        }
    }
}
