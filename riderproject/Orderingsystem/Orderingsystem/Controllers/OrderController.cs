using Microsoft.AspNetCore.Mvc;
using Orderingsystem.Interfaces;
using Orderingsystem.Models;
using Orderingsystem.Models.Requests;

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


    [HttpPost("new/order")]
    public int CreateOrder([FromBody] CreateOrderRequest payload)
    {
        return _orderService.CreateOrder(payload.UserId, payload.AddressId, payload.TotalPrice);
    }

    [HttpPost("new/address")]
    public int CreateAddress([FromBody] CreateAddressRequest payload)
    {
        return _orderService.CreateAddress(payload.AddressLine, payload.PostalNumber, payload.Country);
    }

    [HttpPost("link")]
    public bool AddProductToOrder([FromBody] AddProductToOrderRequest payload)
    {
        return _orderService.AddProductToOrder(payload.OrderId, payload.ProductId, payload.Quantity);
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