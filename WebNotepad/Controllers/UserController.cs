using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebNotepad.Models;
using WebNotepad.Services;

namespace WebNotepad.Controllers;
[ApiController]
public class UserController : ControllerBase
{
    private readonly UsersServices _usersServices;
    private readonly UserManager<User> _userManager;
    public UserController(UsersServices uServices, UserManager<User> userManager)
    {
        _usersServices = uServices;
        _userManager = userManager;
    }
    /*[HttpGet]
    public async Task<string> GetCurrentUserId()
    {
        User usr = await GetCurrentUserAsync();
        return usr?.Id;
    }
    private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    */

    [ActionName("Test")]
    [Authorize]
    [HttpPost("api/test")]
    public IActionResult Test()
    {
        return Ok();
    }
    [ActionName("Create User")]
    [HttpPost("api/users")]
    public IActionResult Create(int id, [FromBody] CreateUserRequest newAcc)
    {
        var  cr =_usersServices.CreateUser(id,newAcc.Email,newAcc.Password);
        return Ok(cr);
    }
    [ActionName("UpdateEmail")]
    [HttpPut("api/users")]
    public async Task<IActionResult> UpdateEmail(int id,string password, string email)
    {
        _usersServices.ChangeEmail(email, await GetCurrentUser());
        return Ok();
    }
    [ActionName("UpdatePassword")]
    [HttpPut("api/users/changepass")]
    public async Task<IActionResult> UpdatePassword([FromBody] ChangeUserPasswordRequest newPass)
    {
        _usersServices.ChangePassword(newPass.oldPassword, newPass.newPassword, newPass.confirmNewPassword, await GetCurrentUser());
        return Ok();
    }
    [ActionName("Login")]
    [HttpPost("api/users/login")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserRequest loginAcc )
    {
       var li = await _usersServices.Login(loginAcc.Email, loginAcc.Password, loginAcc.RememberMe);
       await HttpContext.SignInAsync(li);
       return Ok(li);
    }
    [ActionName("LogOut")]
    [HttpPost("api/users/logout")]
    public async Task<IActionResult> LogOut()
    {
        await _usersServices.Logout();
        return Ok();
    }
    
    [ActionName("Delete User")]
    [HttpDelete("api/users")]
    public IActionResult DeleteUser(int id)
    {
        _usersServices.DeleteUser(id);
        return Ok();
    }
    [ActionName("Show Users")]
    [HttpGet("api/users")]
    public IActionResult ShowUsers()
    {
        return Ok(_usersServices.GetUsers());
    }
    private async Task<User> GetCurrentUser()
    {
        //var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        return await _userManager.GetUserAsync(HttpContext.User);
    }
}