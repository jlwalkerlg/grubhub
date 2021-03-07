namespace Web.Features.Users.RegisterCustomer
{
    public record RegisterCustomerCommand : IRequest
    {
        public string Name { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
