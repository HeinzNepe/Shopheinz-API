using Orderingsystem.Models;

namespace Orderingsystem.Interfaces;

public interface IUserService
{
    public User GetUser(int id);
    public bool CreateUser(string firstName, string lastName, string username, string email, int phoneNumber, string pass, string pfp, int accessLevel);
    public bool DeleteUser(string username);
}