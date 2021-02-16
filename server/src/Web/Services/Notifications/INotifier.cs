using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Domain.Users;

namespace Web.Services.Notifications
{
    public interface INotifier
    {
        Task NotifyCustomerOrderConfirmed(User customer, Order order);
        Task NotifyRestaurantOrderConfirmed(RestaurantManager manager, Order order);
    }
}
