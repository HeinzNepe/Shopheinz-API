using Microsoft.AspNetCore.Mvc;
using Orderingsystem.Interfaces;
using Orderingsystem.Models;

namespace Orderingsystem.Controllers;


[ApiController]
[Route("order")]

public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public Order GetOrder(int id)
    {
        return _orderService.GetOrder(id);
    }
    
    [HttpGet("user")]

    public IEnumerable<Order> GetUserOrders(int id)
    {
        return _orderService.GetUserOrders(id);
    }


    [HttpPost("new")]
    public bool CreateOrder(int userId, int addressId, float totalPrice)
    {
        return _orderService.CreateOrder(userId, addressId, totalPrice);
    }

    [HttpPost("link")]
    public bool AddProductToOrder(int orderId, int productId, int quantity)
    {
        return _orderService.AddProductToOrder(orderId, productId, quantity);
    }

    [HttpPost("update")]
    public bool UpdateStatus(int id, string newStatus)
    {
        return _orderService.UpdateStatus(id, newStatus);
    }

    [HttpDelete("cancel")]
    public bool DeleteOrder(int orderId)
    {
        return _orderService.DeleteOrder(orderId);
    }
}