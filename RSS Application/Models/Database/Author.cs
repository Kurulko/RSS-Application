using System.ComponentModel.DataAnnotations;

namespace RSS_Application.Models.Database;

public class Author
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Enter your {0}!")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Enter your {0}!")]
    public string Surname { get; set; } = null!;

    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [DataType(DataType.Url)]
    public string? Link { get; set; }


    public IEnumerable<Post>? Posts { get; set; }
}
