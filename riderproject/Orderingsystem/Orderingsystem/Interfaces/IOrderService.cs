using Orderingsystem.Models;

namespace Orderingsystem.Interfaces;

public interface IOrderService
{
    public Order GetOrder(int id);
    public IEnumerable<Order> GetUserOrders(int id);

    public bool CreateOrder(int userId, int addressId, float totalPrice);
    public bool AddProductToOrder(int orderId, int productId, int quantity);
    public bool UpdateStatus(int id, string newStatus);
    public bool DeleteOrder(int orderId);

}