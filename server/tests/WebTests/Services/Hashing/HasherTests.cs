using Shouldly;
using Web.Services.Hashing;
using Xunit;

namespace WebTests.Services.Hashing
{
    public class HasherTests
    {
        private readonly Hasher hasher = new();

        [Fact]
        public void CheckMatch_Returns_False_If_The_Hash_Doesnt_Match()
        {
            var hashed = hasher.Hash("wjnvlkwd");

            hasher.CheckMatch("kjerngjner", hashed).ShouldBe(false);
        }

        [Fact]
        public void CheckMatch_Returns_True_If_The_Hash_Matches()
        {
            var hashed = hasher.Hash("raw");

            hashed.ShouldNotBe("raw");
            hasher.CheckMatch("raw", hashed).ShouldBe(true);
        }
    }
}
