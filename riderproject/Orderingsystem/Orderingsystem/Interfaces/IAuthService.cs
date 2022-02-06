namespace Orderingsystem.Interfaces;

public interface IAuthService
{
    public string VerifyCredentials(string user, string pass);
}