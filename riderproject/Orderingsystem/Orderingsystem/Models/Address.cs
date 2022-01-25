namespace Orderingsystem.Models;

public class Address
{
    public int Id { get; set; }
    public string AddressLine { get; set; } = null!;
    public PostalNumber PostalNumber { get; set; } = null!;
    public string Country { get; set; } = null!;
}