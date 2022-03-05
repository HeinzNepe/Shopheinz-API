namespace Orderingsystem.Models.Requests;

public class CreateOrderRequest
{
    public int UserId { get; set; }
    public int AddressId { get; set; }
    public float TotalPrice { get; set; }
}