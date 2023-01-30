using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;
using RSS_Application.Models;
using RSS_Application.Models.Database;
using RSS_Application.Services.PostServices;
using System.ServiceModel.Syndication;
using System.Xml;

namespace RSS_Application.Services.FeedServices;

public class FeedManager : IFeedService
{
    readonly IPostService postService;
    public FeedManager(IPostService postService)
        => this.postService = postService;


    public async Task<string> GetAllFeedDocumentAsync(string host, string userId)
    {
        try
        {
            var posts = await postService.GetPostsAsync();
            return await GetFeedDocumentAsync(host, userId, posts);
        }
        catch
        {
            throw;
        }
    }

    public async Task<string> GetFeedDocumentByIdAsync(string host, string userId, long id)
    {
        try
        {
            Post post = await postService.GetPostByIdAsync(id);
            return await GetFeedDocumentAsync(host, userId, new Post[] { post });
        }
        catch
        {
            throw;
        }

    }

    public async Task<string> GetUnreadedFeedDocumentAsync(string host, string userId)
    {
        try
        {
            var posts = await postService.GetUnreadedPostsAsync(userId);
            return await GetFeedDocumentAsync(host, userId, posts);
        }
        catch
        {
            throw;
        }
    }

    public async Task<string> GetUnreadedFeedDocumentByDateAsync(string host, string userId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var posts = await postService.GetUnreadedPostsByDateAsync(startDate, endDate, userId);
            return await GetFeedDocumentAsync(host, userId, posts);
        }
        catch
        {
            throw;
        }
    }

    public async Task<string> GetReadedFeedDocumentAsync(string host, string userId)
    {
        try
        {
            var posts = await postService.GetReadedPostsAsync(userId);
            return await GetFeedDocumentAsync(host, userId, posts, false);
        }
        catch
        {
            throw;
        }
    }

    public async Task<string> GetReadedFeedDocumentByDateAsync(string host, string userId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var posts = await postService.GetReadedPostsByDateAsync(startDate, endDate, userId);
            return await GetFeedDocumentAsync(host, userId, posts, false);
        }
        catch
        {
            throw;
        }
    }

    public async Task<string> GetFeedDocumentByDateAsync(string host, string userId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var posts = await postService.GetPostsByDateAsync(startDate, endDate);
            return await GetFeedDocumentAsync(host, userId, posts);
        }
        catch
        {
            throw;
        }
    }

    async Task<string> GetFeedDocumentAsync(string host, string userId, IEnumerable<Post> posts, bool isAddReadPosts = true)
    {
        if (isAddReadPosts)
        {
            var newPosts = await GetNewPosts(posts, userId);
            await postService.AddReadedPostsToUserAsync(userId, newPosts);
        }

        IEnumerable<AtomEntry> entries = posts.Select(p => ToRssItem(p, host)).ToList();

        StringWriter sw = new();

        using (XmlWriter xmlWriter = XmlWriter.Create(sw, new() { Async = true, Indent = true }))
        {
            RssFeedWriter rss = new(xmlWriter);
            await rss.WriteTitle("Just title");
            await rss.WriteDescription("Something description");
            await rss.WriteGenerator("Blog");
            await rss.WriteValue("link", host);

            foreach (AtomEntry entry in entries)
                await rss.Write(entry);
        }

        return sw.ToString();
    }

    async Task<IEnumerable<Post>> GetNewPosts(IEnumerable<Post> allPosts, string userId)
        => allPosts.Except(await postService.GetReadedPostsAsync(userId)).ToList();

    AtomEntry ToRssItem(Post post, string host)
    {
        AtomEntry item = new()
        {
            Title = post.Title,
            Description = post.Content,
            Id = $"{host}/posts/{post.Slug}",
            Published = post.CreatedOn,
            LastUpdated = post.CreatedOn,
            ContentType = "html",
        };

        item.AddContributor(new Microsoft.SyndicationFeed.SyndicationPerson(post.Author?.Name ?? "Default name", post.Author?.Email ?? "Default name"));
        item.AddLink(new Microsoft.SyndicationFeed.SyndicationLink(new Uri(item.Id)));
        if (!string.IsNullOrEmpty(post.Category))
            item.AddCategory(new Microsoft.SyndicationFeed.SyndicationCategory(post.Category));

        return item;
    }
}
