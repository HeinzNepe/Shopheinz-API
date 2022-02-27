namespace Orderingsystem.Models.Requests;

public class CreateAddressRequest
{
    public string AddressLine { get; set; } = null!;
    public int PostalNumber { get; set; }
    public string Country { get; set; } = null!;
}