using Web.Services.Hashing;
using Xunit;

namespace InfrastructureTests.Hashing
{
    public class HasherTests
    {
        private readonly Hasher hasher;

        public HasherTests()
        {
            hasher = new Hasher();
        }

        [Fact]
        public void It_Hashes_Strings_And_Checks_That_They_Match()
        {
            var hashed = hasher.Hash("raw");

            Assert.True(hasher.CheckMatch("raw", hashed));
        }

        [Fact]
        public void It_Returns_False_If_The_Given_Hash_Doesnt_Match()
        {
            var hashed = "wjnvlkwd";

            Assert.False(hasher.CheckMatch("kjerngjner", hashed));
        }
    }
}
