namespace Orderingsystem.Interfaces;

public interface IAuthService
{
    public bool VerifyCredentials(string user, string pass);
}