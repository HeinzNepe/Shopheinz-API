using Orderingsystem.Models;

namespace Orderingsystem.Interfaces;

public interface IUserService
{
    public User GetUser(string token);
    public bool CreateUser(string firstName, string lastName, string username, string email, int phoneNumber, string pass, string pfp);
    public bool DeleteUser(string username);
}