using System.ComponentModel.DataAnnotations;

namespace RSS_Application.Models.Database;

public class Post
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Enter the {0}!")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Enter the {0}!")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Enter the {0}!")]
    public string Slug { get; set; } = null!;

    [Required(ErrorMessage = "Enter the {0}!")]
    public string Content { get; set; } = null!;

    [Required(ErrorMessage = "Enter the {0}!")]
    public string Category { get; set; } = null!;

    public DateTime CreatedOn { get; set; }


    public long? AuthorId { get; set; }
    public Author? Author { get; set; }

    public IEnumerable<User>? Users { get; set; }
}
