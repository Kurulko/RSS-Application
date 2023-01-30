using Microsoft.EntityFrameworkCore;
using RSS_Application.Context;
using RSS_Application.Models;
using RSS_Application.Models.Database;

namespace RSS_Application.Services.AuthorServices;

public class AuthorManager : IAuthorService
{
    readonly NewsContext db;
    public AuthorManager(NewsContext db)
        => this.db = db;

    public async Task AddAuthorAsync(Author author)
    {
        try
        {
            await db.Authors.AddAsync(author);
            db.SaveChanges();
        }
        catch
        {
            throw;
        }
    }

    public async Task DeleteAuthorAsync(long authorId)
    {
        try
        {
            Author author = await GetAuthorByIdAsync(authorId);
            db.Authors.Remove(author);
            db.SaveChanges();
        }
        catch
        {
            throw;
        }
    }

    public Task<Author> GetAuthorByIdAsync(long authorId)
    {
        try
        {
            return Task.FromResult(GetAuthorsIncludesPosts().First(a => a.Id == authorId))!;
        }
        catch
        {
            throw;
        }
    }

    public Task<IEnumerable<Author>> GetAuthorsAsync()
    {
        try
        {
            return Task.FromResult(GetAuthorsIncludesPosts());
        }
        catch
        {
            throw;
        }
    }

    public Task UpdateAuthorAsync(Author author)
    {
        try
        {
            db.Authors.Update(author);
            db.SaveChanges();
            return Task.CompletedTask;
        }
        catch
        {
            throw;
        }
    }

    IEnumerable<Author> GetAuthorsIncludesPosts()
        => db.Authors.Include(a => a.Posts).AsEnumerable();
}
