using Microsoft.AspNetCore.Mvc;
using Orderingsystem.Interfaces;

namespace Orderingsystem.Controllers;

[ApiController]
[Route("user")]

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("create")]
    public bool CreateUser(string firstName, string lastName, string username, string email, int phoneNumber, string pass, int accessLevel)
    {
        return _userService.CreateUser(firstName, lastName, username, email, phoneNumber, pass, accessLevel);
    }
}