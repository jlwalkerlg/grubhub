namespace FoodSnap.Application.Validation
{
    public class MinLengthFailure : IValidationFailure
    {
        public MinLengthFailure(int length)
        {
            Length = length;
        }

        public int Length { get; }
    }
}
