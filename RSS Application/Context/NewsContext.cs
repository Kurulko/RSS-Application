using Microsoft.EntityFrameworkCore;
using RSS_Application.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RSS_Application.Models.Database;

namespace RSS_Application.Context;

public class NewsContext : IdentityDbContext<User>
{
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;

    public NewsContext(DbContextOptions<NewsContext> opts) : base(opts)
        => Database.EnsureCreated();
}
