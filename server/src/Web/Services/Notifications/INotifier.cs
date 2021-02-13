using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Domain.Users;

namespace Web.Services.Notifications
{
    public interface INotifier
    {
        Task NotifyOrderConfirmed(User user, Order order);
    }
}
