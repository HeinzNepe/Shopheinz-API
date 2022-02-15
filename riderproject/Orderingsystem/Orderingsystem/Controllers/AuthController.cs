using Microsoft.AspNetCore.Mvc;
using Orderingsystem.Interfaces;
using Orderingsystem.Models;
using Orderingsystem.Models.Requests;

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
    public string VerifyCredentials([FromBody] VerifyRequest payload)
    {
        return _authService.VerifyCredentials(payload.User, payload.Pass);
    }

    [HttpPost]
    public bool UpdatePass([FromBody] UpdateCredentialsRequest payload)
    {
        return _authService.UpdatePass(payload.Token, payload.Pass, payload.NewPass);
    }
    
}