using RSS_Application.Models;
using RSS_Application.Models.Database;

namespace RSS_Application.Services.PostServices;

public interface IPostService
{
    Task<IEnumerable<Post>> GetPostsAsync();
    Task<IEnumerable<Post>> GetPostsByDateAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Post>> GetReadedPostsAsync(string userId);
    Task<IEnumerable<Post>> GetReadedPostsByDateAsync(DateTime startDate, DateTime endDate, string userId);
    Task<IEnumerable<Post>> GetUnreadedPostsAsync(string userId);
    Task<IEnumerable<Post>> GetUnreadedPostsByDateAsync(DateTime startDate, DateTime endDate, string userId);
    Task<Post> GetPostByIdAsync(long postId);
    Task AddPostAsync(Post post);
    Task UpdatePostAsync(Post post);
    Task DeletePostAsync(long postId);
    Task AddReadedPostsToUserAsync(string userId, IEnumerable<Post> posts);
}
