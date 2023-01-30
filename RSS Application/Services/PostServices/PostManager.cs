using Microsoft.EntityFrameworkCore;
using RSS_Application.Context;
using RSS_Application.Models;
using RSS_Application.Models.Database;

namespace RSS_Application.Services.PostServices;

public class PostService : IPostService
{
    readonly NewsContext db;
    public PostService(NewsContext db)
        => this.db = db;

    public async Task AddPostAsync(Post post)
    {
        try
        {
            await db.Posts.AddAsync(post);
            db.SaveChanges();
        }
        catch
        {
            throw;
        }
    }

    public async Task AddReadedPostsToUserAsync(string userId, IEnumerable<Post> posts)
    {
        try
        {
            User user = await GetUserIncludesReadedPost(userId);

            foreach (Post post in posts)
                user.ReadedPosts!.Add(post);
            db.SaveChanges();
        }
        catch
        {
            throw;
        }
    }

    public async Task DeletePostAsync(long postId)
    {
        try
        {
            Post post = await GetPostByIdAsync(postId);
            db.Posts.Remove(post);
            db.SaveChanges();
        }
        catch
        {
            throw;
        }
    }

    public Task<Post> GetPostByIdAsync(long postId)
    {
        try
        {
            return Task.FromResult(GetPostsIncludesAuthors().First(p => p.Id == postId))!;
        }
        catch
        {
            throw;
        }
    }

    public Task<IEnumerable<Post>> GetPostsAsync()
    {
        try
        {
            return Task.FromResult(GetPostsIncludesAuthors());
        }
        catch
        {
            throw;
        }
    }

    public Task<IEnumerable<Post>> GetPostsByDateAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            return Task.FromResult(GetPostsIncludesAuthors().Where(p => IsSuitableDate(p.CreatedOn, startDate, endDate)).AsEnumerable()!);
        }
        catch
        {
            throw;
        }
    }

    public async Task<IEnumerable<Post>> GetReadedPostsAsync(string userId)
    {
        try
        {
            return (await GetUserIncludesReadedPost(userId)).ReadedPosts?.AsEnumerable()!;
        }
        catch
        {
            throw;
        }
    }


    public async Task<IEnumerable<Post>> GetReadedPostsByDateAsync(DateTime startDate, DateTime endDate, string userId)
    {
        try
        {
            return (await GetReadedPostsAsync(userId)).Where(p => IsSuitableDate(p.CreatedOn, startDate, endDate)).AsEnumerable()!;
        }
        catch
        {
            throw;
        }
    }

    public async Task<IEnumerable<Post>> GetUnreadedPostsAsync(string userId)
    {
        try
        {
            return GetPostsIncludesAuthors().Except(await GetReadedPostsAsync(userId)).AsEnumerable()!;
        }
        catch
        {
            throw;
        }
    }

    public async Task<IEnumerable<Post>> GetUnreadedPostsByDateAsync(DateTime startDate, DateTime endDate, string userId)
    {
        try
        {
            return (await GetUnreadedPostsAsync(userId)).Where(p => IsSuitableDate(p.CreatedOn, startDate, endDate)).AsEnumerable()!;
        }
        catch
        {
            throw;
        }
    }

    public Task UpdatePostAsync(Post post)
    {
        try
        {
            db.Posts.Update(post);
            db.SaveChanges();
            return Task.CompletedTask;
        }
        catch
        {
            throw;
        }
    }


    async Task<User> GetUserIncludesReadedPost(string userId)
        => await db.Users.Include(u => u.ReadedPosts)!.ThenInclude(p => p.Author).FirstAsync(u => u.Id == userId);
    IEnumerable<Post> GetPostsIncludesAuthors()
        => db.Posts.Include(p => p.Author).AsEnumerable();
    bool IsSuitableDate(DateTime currentDate, DateTime startDate, DateTime endDate)
        => currentDate >= startDate && currentDate <= endDate;
}
