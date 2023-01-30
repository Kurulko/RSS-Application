using System.ComponentModel.DataAnnotations;

namespace RSS_Application.Models.Account;

public abstract class AccountModel
{
    [Required(ErrorMessage = "Enter your name!")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Enter your password!")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "{0} must be at least {1} characters long")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
}
