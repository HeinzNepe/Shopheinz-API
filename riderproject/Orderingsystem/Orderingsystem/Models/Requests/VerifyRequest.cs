namespace Orderingsystem.Models.Requests;

public class VerifyRequest
{
    public string User { get; set; } = null!;
    public string Pass { get; set; } = null!;
}