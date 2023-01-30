using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RSS_Application.Models.Database;

public class User : IdentityUser
{
    public DateTime RegisteredTime { get; set; }

    public IList<Post>? ReadedPosts { get; set; }
}
