namespace FoodSnap.Application.Validation.Failures
{
    public class EmailTakenFailure : IValidationFailure
    {
        public string Message { get; }

        public EmailTakenFailure()
        {
        }

        public EmailTakenFailure(string message)
        {
            Message = message;
        }
    }
}
