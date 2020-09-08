using System;
using FoodSnap.Web.Services.Tokenization;
using Xunit;

namespace FoodSnap.WebTests.Services.Tokenization
{
    public class JWTTokenizerTests
    {
        private readonly JWTTokenizer tokenizer;

        public JWTTokenizerTests()
        {
            tokenizer = new JWTTokenizer("secret");
        }

        [Fact]
        public void It_Tokenizes_Strings()
        {
            var id = Guid.NewGuid().ToString();

            var token = tokenizer.Encode(id);
            Assert.Equal(id, tokenizer.Decode(token).Value);
        }

        [Fact]
        public void It_Returns_An_Error_If_The_Token_Is_Invalid()
        {
            var token = "some random token";

            Assert.False(tokenizer.Decode(token).IsSuccess);
        }
    }
}
