namespace Web.Features.Users.RegisterCustomer
{
    public record RegisterCustomerCommand : IRequest
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
