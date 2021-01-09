using System;
using Shouldly;
using Web.Services.Tokenization;
using Xunit;

namespace WebTests.Services.Tokenization
{
    public class JWTTokenizerTests
    {
        private readonly JWTTokenizer tokenizer = new("secret");

        [Fact]
        public void It_Tokenizes_Strings()
        {
            var id = Guid.NewGuid().ToString();

            var token = tokenizer.Encode(id);

            token.ShouldNotBe(id);

            var result = tokenizer.Decode(token);

            result.IsSuccess.ShouldBe(true);
            result.Value.ShouldBe(id);
        }

        [Fact]
        public void It_Returns_An_Error_If_The_Token_Is_Invalid()
        {
            var token = "some random token";

            var result = tokenizer.Decode(token);

            result.ShouldBeAnError();
        }
    }
}
