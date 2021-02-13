using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs
{
    [Authorize]
    public class OrderHub : Hub
    {
    }
}
