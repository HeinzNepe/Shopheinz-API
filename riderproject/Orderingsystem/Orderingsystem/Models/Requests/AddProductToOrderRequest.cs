namespace Orderingsystem.Models.Requests;

public class AddProductToOrderRequest
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}