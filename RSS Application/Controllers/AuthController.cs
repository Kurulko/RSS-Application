using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSS_Application.Models.Account;
using RSS_Application.Services.AuthServices;

namespace RSS_Application.Controllers;

[AllowAnonymous]
public class AuthController : BaseApiController
{
    readonly IAuthService accManager;
    public AuthController(IAuthService accManager)
        => this.accManager = accManager;

    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register([FromBody] RegisterModel register)
        => await ReturnOkIfEverithingIsGood(async () => await accManager.RegisterUserAsync(register));


    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
        => await ReturnOkIfEverithingIsGood(async () => await accManager.LoginUserAsync(login));


    [Authorize]
    [HttpPost(nameof(Logout))]
    public async Task<IActionResult> Logout()
        => await ReturnOkIfEverithingIsGood(async () => await accManager.LogoutUserAsync());
}
