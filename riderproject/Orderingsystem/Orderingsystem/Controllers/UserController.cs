using Microsoft.AspNetCore.Mvc;
using Orderingsystem.Interfaces;
using Orderingsystem.Models;
using Orderingsystem.Models.Requests;

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
    public User GetUser([FromHeader]string token)
    {
        return _userService.GetUser(token);
    }
    
    [HttpPost("create")]
    public bool CreateUser([FromBody] CreateUserRequest payload)
    {
        return _userService.CreateUser(payload.FirstName, payload.LastName, payload.Username, payload.Email, payload.PhoneNumber, payload.Pass, payload.Pfp);
    }

    [HttpDelete("delete")]
    public bool DeleteUser(string username)
    {
        return _userService.DeleteUser(username);
    }

    [HttpPost("update")]
    public bool UpdateUser([FromBody] UpdateUserRequest payload)
    {
        return _userService.UpdateUser(payload.Token, payload.FirstName, payload.LastName, payload.Email, payload.PhoneNumber, payload.Pfp);
    }
}