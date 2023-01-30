namespace RSS_Application.Services.Initializer.Models;

public abstract class ModelsInitializer
{
    public async Task InitializeAsync()
    {
        await AuthorsInitializeAsync();
        await PostsInitializeAsync();
    }
    protected abstract Task PostsInitializeAsync();
    protected abstract Task AuthorsInitializeAsync();
}
