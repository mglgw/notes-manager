using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebNotepad.Models;
using WebNotepad.Services;

namespace WebNotepad.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly UserService _userService;
    public UserController(UserService uServices, UserManager<User> userManager)
    {
        _userService = uServices;
        _userManager = userManager;
    }
    [ActionName("Test")]
    [Authorize]
    [HttpPost]
    public IActionResult Test()
    {
        return Ok();
    }
    [ActionName("Create User")]
    [HttpPost]
    public IActionResult Create([FromBody] CreateUserRequest newUserRequest)
    {
        var user = _userService.CreateUser(newUserRequest.Email, newUserRequest.Password);
        return Ok(user);
    }
    [ActionName("UpdateEmail")]
    [HttpPut]
    public async Task<IActionResult> UpdateEmail(string email)
    {
        await _userService.ChangeEmail(email, await GetCurrentUser());
        return Ok();
    }
    [ActionName("UpdatePassword")]
    [HttpPut("/change-pass")]
    public async Task<IActionResult> UpdatePassword([FromBody] ChangeUserPasswordRequest updatePasswordRequest)
    {
        await _userService.ChangePassword(updatePasswordRequest.oldPassword,
            updatePasswordRequest.newPassword,
            updatePasswordRequest.confirmNewPassword,
            await GetCurrentUser());
        return Ok();
    }
    [ActionName("Login")]
    [HttpPost("/login")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserRequest loginRequest)
    {
        var claimsPrincipal =
            await _userService.Login(loginRequest.Email, loginRequest.Password, loginRequest.RememberMe);
        await HttpContext.SignInAsync(claimsPrincipal);
        return Ok(claimsPrincipal);
    }
    [ActionName("LogOut")]
    [HttpPost("/logout")]
    public async Task<IActionResult> LogOut()
    {
        await _userService.Logout();
        return Ok();
    }

    [ActionName("Show Users")]
    [HttpGet]
    public IActionResult ShowUsers()
    {
        return Ok(_userService.GetUsers());
    }
    private async Task<User> GetCurrentUser()
    {
        return await _userManager.GetUserAsync(HttpContext.User);
    }
}