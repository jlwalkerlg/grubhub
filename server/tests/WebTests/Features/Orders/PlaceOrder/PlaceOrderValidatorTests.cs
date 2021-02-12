using System.Threading.Tasks;
using Shouldly;
using Web.Features.Orders.PlaceOrder;
using Xunit;

namespace WebTests.Features.Orders.PlaceOrder
{
    public class PlaceOrderValidatorTests
    {
        private readonly PlaceOrderValidator validator;

        public PlaceOrderValidatorTests()
        {
            validator = new PlaceOrderValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("09834562")]
        public async Task Mobile(string mobile)
        {
            var command = new PlaceOrderCommand()
            {
                Mobile = mobile,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Mobile));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task AddressLine1(string address)
        {
            var command = new PlaceOrderCommand()
            {
                AddressLine1 = address,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.AddressLine1));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task City(string city)
        {
            var command = new PlaceOrderCommand()
            {
                City = city,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.City));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        public async Task Postcode(string postcode)
        {
            var command = new PlaceOrderCommand()
            {
                Postcode = postcode,
            };

            var result = await validator.Validate(command);

            result.ShouldBeAnError();
            result.Errors.ShouldContainKey(nameof(command.Postcode));
        }
    }
}
