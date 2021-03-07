namespace Web.Features.Users.UpdateAccountDetails
{
    public record UpdateAccountDetailsCommand : IRequest
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string MobileNumber { get; init; }
    }
}
