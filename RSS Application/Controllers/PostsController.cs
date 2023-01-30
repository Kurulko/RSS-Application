using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSS_Application.Models;
using RSS_Application.Models.Database;
using RSS_Application.Role;
using RSS_Application.Services.PostServices;

namespace RSS_Application.Controllers;

[Authorize(Roles = Roles.Admin)]
public class PostsController : BaseApiController
{
    readonly IPostService postService;
    public PostsController(IPostService postService)
        => this.postService = postService;

    [HttpGet]
    public async Task<IEnumerable<Post>> GetPostsAsync()
        => (await postService.GetPostsAsync()).Select(p => PostForJsonFormat(p));

    [HttpGet("{postId:long}")]
    public async Task<Post> GetPostByIdAsync(long postId)
        => PostForJsonFormat(await postService.GetPostByIdAsync(postId));

    [HttpPost]
    public async Task<IActionResult> AddPostAsync([FromBody] Post post)
        => await ReturnOkIfEverithingIsGood(async () => await postService.AddPostAsync(post));

    [HttpPut]
    public async Task<IActionResult> UpdatePostAsync([FromBody] Post post)
        => await ReturnOkIfEverithingIsGood(async () => await postService.UpdatePostAsync(post));

    [HttpDelete("{postId:long}")]
    public async Task<IActionResult> DeletePostAsync(long postId)
        => await ReturnOkIfEverithingIsGood(async () => await postService.DeletePostAsync(postId));

    Post PostForJsonFormat(Post post)
    {
        if(post.Author is not null)
            post.Author!.Posts = null;
        return post;
    }
}
