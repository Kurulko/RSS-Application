using RSS_Application.Models;
using RSS_Application.Models.Database;

namespace RSS_Application.Services.AuthorServices;

public interface IAuthorService
{
    Task<IEnumerable<Author>> GetAuthorsAsync();
    Task<Author> GetAuthorByIdAsync(long authorId);
    Task AddAuthorAsync(Author author);
    Task UpdateAuthorAsync(Author author);
    Task DeleteAuthorAsync(long authorId);
}
