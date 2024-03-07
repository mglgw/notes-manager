using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebNotepad.Models;

namespace WebNotepad.Services;

public class UserService
{
    private readonly DataBaseContext _dataBaseContext;
    private readonly IOptions<IdentityOptions> _identityOptions;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    public UserService(DataBaseContext dataBase,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IOptions<IdentityOptions> identityOptions)
    {
        _dataBaseContext = dataBase;
        _userManager = userManager;
        _signInManager = signInManager;
        _identityOptions = identityOptions;
    }
    public async Task<IdentityResult> CreateUser(string email, string password)
    {
        var user = new User
            { UserName = email, Email = email };

        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            return result;
        }
        throw new Exception(result.Errors.ToString());
    }
    public async Task<IdentityResult> ChangePassword(string oldPass, string newPass, string confNewPass, User user)
    {
        if (newPass != confNewPass)
        {
            throw new Exception("Error occured");
        }

        var result = await _userManager.ChangePasswordAsync(user, oldPass, newPass);
        return result;
    }
    public async Task ChangeEmail(string newEmail, User user)
    {
        string? token = await _userManager.GenerateUserTokenAsync(user, "EmailChange", "ChangingEmail");
        await _userManager.ChangeEmailAsync(user, newEmail, token);
    }
    public async Task<ClaimsPrincipal> Login(string email, string password, bool rememberMe)
    {
        var result = await _signInManager.PasswordSignInAsync(email,
            password, rememberMe, true);
        var user = await _userManager.FindByEmailAsync(email);
        if (result.Succeeded && user != null)
        {
            var factory = new UserClaimsPrincipalFactory<User>(_userManager, _identityOptions);
            var claims = await factory.CreateAsync(user);
            return claims;
        }
        if (result.IsLockedOut)
        {
            throw new Exception(result.IsLockedOut.ToString());
        }
        {
            throw new Exception("Login failed");
        }
    }
    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public List<User> GetUsers()
    {
        return _dataBaseContext.Users.ToList();
    }
}