using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Features.Baskets.UpdateBasketItemQuantity;
using Xunit;

namespace WebTests.Features.Baskets.UpdateBasketItemQuantity
{
    public class UpdateBasketItemQuantityValidatorTests
    {
        private readonly UpdateBasketItemQuantityValidator validator;

        public UpdateBasketItemQuantityValidatorTests()
        {
            validator = new UpdateBasketItemQuantityValidator();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Quantity_Must_Be_Greater_Than_Zero(int quantity)
        {
            var command = new UpdateBasketItemQuantityCommand()
            {
                Quantity = quantity,
            };

            var result = await validator.Validate(command);
            
            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Quantity));
        }
    }
}