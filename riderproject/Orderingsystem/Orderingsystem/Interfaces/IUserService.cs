namespace Orderingsystem.Interfaces;

public interface IUserService
{
    public bool CreateUser(string firstName, string lastName, string username, string email, int phoneNumber, string pass, int accessLevel);
}