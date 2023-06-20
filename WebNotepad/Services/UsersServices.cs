using System.Security.Claims;
using Intro;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebNotepad.Models;


namespace WebNotepad.Services;

public class UsersServices
{
    private readonly DataBaseContext _dataBaseContext;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<UserRole> _roleManager;
    private readonly IOptions<IdentityOptions> _identityOptions;
    public UsersServices(DataBaseContext DataBase, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<UserRole> roleManager, IOptions<IdentityOptions> identityOptions)
    {
        _dataBaseContext =DataBase;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _identityOptions = identityOptions;
    }
    public async Task <IdentityResult> CreateUser(int id, string email, string password)
    {
        var tempuser = new User
        {
            UserName = email,
            Email = email,
            
        };

        var result = await _userManager.CreateAsync(tempuser, password);
        if (result.Succeeded)
        {
            return result;
        }
        else
        {
            throw new Exception(result.Errors.ToString());
        }
    }
    public void DeleteUser(int tempid)
    {
        /*var tempuser = _dataBaseContext.Users
            .Where(user => tempid == user.Id)
            .FirstOrDefault();
        _dataBaseContext.Users.Remove(tempuser);
        _dataBaseContext.SaveChanges();*/
    }
    public async Task<IdentityResult> ChangePassword( string oldPass, string newPass, string confNewPass, User user)
    {
        if (newPass == confNewPass && user != null)
        {
         var result =  await _userManager.ChangePasswordAsync(user,oldPass, newPass);
         return result;
        }
        else
        {
            throw new Exception("Error occured");
        }
    }
    public async Task ChangeEmail(string newEmail, User user)
    {
        var tempToken = await _userManager.GenerateUserTokenAsync(user, "EmailChange", "ChangingEmail");
        await _userManager.ChangeEmailAsync(user, newEmail, tempToken);
    }
    public async Task<ClaimsPrincipal> Login( string tempemail, string password, bool rememberme)
    {
        var result = await _signInManager.PasswordSignInAsync(tempemail,
            password, rememberme, lockoutOnFailure: true);
        var user = await _userManager.FindByEmailAsync(tempemail);
        var userRole = await _userManager.GetRolesAsync(user);
        if (result.Succeeded && user != null)
        {
            var factory = new UserClaimsPrincipalFactory<User>(_userManager, _identityOptions);
            var claims = await factory.CreateAsync(user);
            return claims;
        }
        else if (result.IsLockedOut)
        {
            throw new Exception(result.IsLockedOut.ToString());
        }
        else
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
    private string EncPass(string password)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        MD5 md5 = MD5.Create();
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        string hashstring = BitConverter.ToString(hashBytes).Replace("-", "");
        return hashstring;
    }
}