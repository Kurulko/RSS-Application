using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSS_Application.Models;
using RSS_Application.Models.Database;
using RSS_Application.Role;
using RSS_Application.Services.AuthorServices;

namespace RSS_Application.Controllers;


[Authorize(Roles = Roles.Admin)]
public class AuthorsController : BaseApiController
{
    readonly IAuthorService authorService;
    public AuthorsController(IAuthorService authorService)
        => this.authorService = authorService;

    [HttpGet]
    public async Task<IEnumerable<Author>> GetAuthorsAsync()
        => (await authorService.GetAuthorsAsync()).Select(a => AuthorForJsonFormat(a));

    [HttpGet("{authorId:long}")]
    public async Task<Author> GetAuthorByIdAsync(long authorId)
        => AuthorForJsonFormat(await authorService.GetAuthorByIdAsync(authorId));

    [HttpPost]
    public async Task<IActionResult> AddAuthorAsync([FromBody] Author author)
        => await ReturnOkIfEverithingIsGood(async () => await authorService.AddAuthorAsync(author));

    [HttpPut]
    public async Task<IActionResult> UpdateAuthorAsync([FromBody] Author author)
        => await ReturnOkIfEverithingIsGood(async () => await authorService.UpdateAuthorAsync(author));

    [HttpDelete("{authorId:long}")]
    public async Task<IActionResult> DeleteAuthorAsync(long authorId)
        => await ReturnOkIfEverithingIsGood(async () => await authorService.DeleteAuthorAsync(authorId));

    Author AuthorForJsonFormat(Author author)
    {
        if (author.Posts is not null)
        {
            foreach (Post post in author!.Posts)
            {
                if (post.Author is not null)
                    post.Author = null;
            }
        }

        return author;
    }
}
