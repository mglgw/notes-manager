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
        /*
        var tempuser = new User();
        tempuser.Id = id;
        //tempuser.Name = name;
        tempuser.Email = email;
        tempuser.Password = EncPass(password);
        _dataBaseContext.Users.Add(tempuser);
        _dataBaseContext.SaveChanges();*/
    }
    public void DeleteUser(int tempid)
    {
        /*var tempuser = _dataBaseContext.Users
            .Where(user => tempid == user.Id)
            .FirstOrDefault();
        _dataBaseContext.Users.Remove(tempuser);
        _dataBaseContext.SaveChanges();*/
    }
    public void ChangePassword(string email, string oldpass, string newpass, string confnewpass)
    {/*
        string temppass = EncPass(newpass);
        string temppassconf = EncPass(confnewpass);
        oldpass = EncPass(oldpass);
        var tempuser = _dataBaseContext.Users
            .Where(user => email == user.Email)
            .FirstOrDefault();
        if (temppass == temppassconf )
        {
            if (tempuser == null || tempuser.Password != oldpass)
            {
                throw new Exception("User does not exist or credentials are incorrect!");
            }
            tempuser.Password = temppass;
            _dataBaseContext.SaveChanges();
        }
        else
        {
            throw new Exception("New passwords do not match!");
        }
        */
       
    }
    public void ChangeEmail(int tempid, string password, string newemail)
    {/*
        password = EncPass(password);
        var tempuser = _dataBaseContext.Users
            .Where(user => tempid == user.Id)
            .FirstOrDefault();
        if (tempuser == null || tempuser.Password !=password)
        {
            throw new Exception("User does not exist or credentials are incorrect!");
        }
        tempuser.Email = newemail;
        _dataBaseContext.SaveChanges();*/
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

        /*var encpassword = EncPass(password);
        var tempuser = _dataBaseContext.Users
            .Where(user => tempemail == user.Email)
            .FirstOrDefault();
        if (tempuser == null || tempuser.Password != encpassword)
        {
            throw new Exception("User does not exist or credentials are incorrect!");
        }*/
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