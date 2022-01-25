using Orderingsystem.Models;

namespace Orderingsystem.Interfaces;

public interface IOrderService
{
    public Order GetOrder(int id);
    
    
}