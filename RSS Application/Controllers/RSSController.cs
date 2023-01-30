using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Syndication;
using System.Xml;
using System.IO;
using System.Text;
using RSS_Application.Services;
using RSS_Application.Services.FeedServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RSS_Application.Models.Database;

namespace RSS_Application.Controllers;

[Authorize]
public class RSSController : BaseApiController
{
    readonly IFeedService feedService;
    readonly UserManager<User> userManager;
    public RSSController(IFeedService feedService, UserManager<User> userManager)
        => (this.feedService, this.userManager) = (feedService, userManager);

    string Host => $"{Request.Scheme}://{Request.Host}";
    string UserId => userManager.GetUserId(base.User);


    [HttpGet]
    public async Task<IActionResult> GetAllFeedDocumentAsync()
    {
        string content = await feedService.GetAllFeedDocumentAsync(Host, UserId);
        return ContentRss(content);
    }

    [HttpGet("readed")]
    public async Task<IActionResult> GetReadedFeedDocumentAsync()
    {
        string content = await feedService.GetReadedFeedDocumentAsync(Host, UserId);
        return ContentRss(content);
    }

    [HttpGet("readed/date")]
    public async Task<IActionResult> GetReadedFeedDocumentByDateAsync(DateTime startDate, DateTime endDate)
    {
        string content = await feedService.GetReadedFeedDocumentByDateAsync(Host, UserId, startDate, endDate);
        return ContentRss(content);
    }

    [HttpGet("unreaded")]
    public async Task<IActionResult> GetUnreadedFeedDocumentAsync()
    {
        string content = await feedService.GetUnreadedFeedDocumentAsync(Host, UserId);
        return ContentRss(content);
    }

    [HttpGet("unreaded/date")]
    public async Task<IActionResult> GetUnreadedFeedDocumentByDateAsync(DateTime startDate, DateTime endDate)
    {
        string content = await feedService.GetUnreadedFeedDocumentByDateAsync(Host, UserId, startDate, endDate);
        return ContentRss(content);
    }

    [HttpGet("date")]
    public async Task<IActionResult> GetFeedDocumentByDateAsync(DateTime startDate, DateTime endDate)
    {
        string content = await feedService.GetFeedDocumentByDateAsync(Host, UserId, startDate, endDate);
        return ContentRss(content);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetFeedDocumentByDateAsync(long id)
    {
        string content = await feedService.GetFeedDocumentByIdAsync(Host, UserId, id);
        return ContentRss(content);
    }

    IActionResult ContentRss(string content)
    {
        string contentType = "application/xml";
        return Content(content, contentType);
    }
}
