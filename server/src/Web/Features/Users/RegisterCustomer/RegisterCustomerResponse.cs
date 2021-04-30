namespace Web.Features.Users.RegisterCustomer
{
    public record RegisterCustomerResponse
    {
        public string XsrfToken { get; init; }
    }
}
