namespace WebNotepad.Models;

public class ChangeUserPasswordRequest
{
    public string email { get; set; }
    public string oldPassword { get; set; }
    public string newPassword { get; set; }
    public string confirmNewPassword { get; set; }
}