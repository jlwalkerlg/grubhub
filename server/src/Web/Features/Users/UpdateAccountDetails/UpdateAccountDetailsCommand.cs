namespace Web.Features.Users.UpdateAccountDetails
{
    public record UpdateAccountDetailsCommand : IRequest
    {
        public string Name { get; init; }
        public string MobileNumber { get; init; }
    }
}
