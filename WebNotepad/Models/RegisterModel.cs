using System.ComponentModel.DataAnnotations;

namespace WebNotepad.Models;

public class RegisterModel
{
    [Required(ErrorMessage = "User is required")]
    public string? userName { get; set; }
    
    [Required(ErrorMessage = "E-mail is required")]
    public string? email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string? password { get; set; }
}