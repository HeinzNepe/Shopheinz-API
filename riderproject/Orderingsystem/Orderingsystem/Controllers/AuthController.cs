using Microsoft.AspNetCore.Mvc;
using Orderingsystem.Interfaces;
using Orderingsystem.Models;

namespace Orderingsystem.Controllers;


[ApiController]
[Route("auth")]

public class AuthController : Controller
{

    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public string VerifyCredentials([FromHeader]string user, [FromHeader]string pass)
    {
        return _authService.VerifyCredentials(user, pass);
    }

    [HttpPost]
    public bool UpdatePass([FromHeader]string user, [FromHeader]string pass, [FromHeader]string newPass)
    {
        return _authService.UpdatePass(user, pass, newPass);
    }
    
}