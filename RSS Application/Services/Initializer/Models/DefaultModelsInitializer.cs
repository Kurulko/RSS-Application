using RSS_Application.Models;
using RSS_Application.Models.Database;
using RSS_Application.Services;
using RSS_Application.Services.AuthorServices;
using RSS_Application.Services.PostServices;

namespace RSS_Application.Services.Initializer.Models;

public class DefaultModelsInitializer : ModelsInitializer
{
    readonly IPostService postService;
    readonly IAuthorService authorService;
    public DefaultModelsInitializer(IPostService postService, IAuthorService authorService)
        => (this.postService, this.authorService) = (postService, authorService);

    const int countOfPosts = 50;
    const int countOfAuthors = 10;

    protected override async Task PostsInitializeAsync()
    {
        var defaultPosts = GetDefaultPosts();
        foreach (Post post in defaultPosts)
            await postService.AddPostAsync(post);
    }
    IEnumerable<Post> GetDefaultPosts()
    {
        IList<Post> posts = new List<Post>();

        string title = "Title", description = "Description", slug = "Slug", content = "Content", category = "Category";
        for (int i = 1; i <= countOfPosts; i++)
        {
            Random random = new();
            DateTime createdOn = DateTime.Now.AddDays(-random.Next(20));
            long authorId = random.Next(countOfAuthors) + 1;
            posts.Add(new Post() { Title = title + i, Description = description + i, Slug = slug + i, Content = content + i, Category = category + i, CreatedOn = createdOn, AuthorId = authorId });
        }

        return posts;
    }

    protected override async Task AuthorsInitializeAsync()
    {
        var defaultAuthors = GetDefaultAuthors();
        foreach (Author author in defaultAuthors)
            await authorService.AddAuthorAsync(author);
    }
    IEnumerable<Author> GetDefaultAuthors()
    {
        IList<Author> authors = new List<Author>();

        string name = "Name", surname = "Surname", email = "myemail", link = "https://somesite/authors/name";
        for (int i = 1; i <= countOfAuthors; i++)
            authors.Add(new Author() { Name = name + i, Surname = surname + i, Email = $"{email}{i}@gmail.com", Link = link + i });

        return authors;
    }
}
