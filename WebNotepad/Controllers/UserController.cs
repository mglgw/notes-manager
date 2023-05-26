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
    public IActionResult UpdateEmail(int id,string password, string email)
    {
        _usersServices.ChangeEmail(id, password, email);
        return Ok();
    }
    [ActionName("UpdatePassword")]
    [HttpPut("api/users/changepass")]
    public IActionResult UpdatePassword([FromBody] ChangeUserPasswordRequest newPass)
    {
        _usersServices.ChangePassword(newPass.email, newPass.oldPassword, newPass.newPassword, newPass.confirmNewPassword);
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
}