using RSS_Application.Models.Database;
using System.ComponentModel.DataAnnotations;

namespace RSS_Application.Models.Account;

public class RegisterModel : AccountModel
{
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Repeat password")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least {1} characters long")]
    [Compare("Password", ErrorMessage = "Passwords don't match")]
    public string PasswordConfirm { get; set; } = null!;

    public static explicit operator User(RegisterModel register)
        => new() { Email = register.Email, UserName = register.Name, RegisteredTime = DateTime.Now };
}
