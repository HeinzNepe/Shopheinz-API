using Microsoft.AspNetCore.Mvc;
using Orderingsystem.Interfaces;
using Orderingsystem.Models;

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

    [HttpGet("user")]
    public User GetUser(string token)
    {
        return _userService.GetUser(token);
    }
    
    [HttpPost("create")]
    public bool CreateUser([FromHeader]string firstName, [FromHeader]string lastName, [FromHeader]string username, [FromHeader]string email, [FromHeader]int phoneNumber, [FromHeader]string pass, [FromHeader]string pfp, int accessLevel)
    {
        return _userService.CreateUser(firstName, lastName, username, email, phoneNumber, pass, pfp, accessLevel);
    }

    [HttpDelete("delete")]
    public bool DeleteUser(string username)
    {
        return _userService.DeleteUser(username);
    }
}