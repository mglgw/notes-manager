namespace WebNotepad.Models;

public class AuthenticateUserRequest
{
    public string Password { get; set; }
    public string Email { get; set; }

    public bool RememberMe { get; set; }
}