using RSS_Application.Models.Database;

namespace RSS_Application.Services.FeedServices;

public interface IFeedService
{
    Task<string> GetAllFeedDocumentAsync(string host, string userId);
    Task<string> GetFeedDocumentByIdAsync(string host, string userId, long id);
    Task<string> GetFeedDocumentByDateAsync(string host, string userId, DateTime startDate, DateTime endDate);
    Task<string> GetUnreadedFeedDocumentAsync(string host, string userId);
    Task<string> GetUnreadedFeedDocumentByDateAsync(string host, string userId, DateTime startDate, DateTime endDate);
    Task<string> GetReadedFeedDocumentAsync(string host, string userId);
    Task<string> GetReadedFeedDocumentByDateAsync(string host, string userId, DateTime startDate, DateTime endDate);
}
