using Microsoft.EntityFrameworkCore;
using RSS_Application.Services.PostServices;
using RSS_Application.Services.Initializer;
using RSS_Application.Services.FeedServices;
using RSS_Application.Services.AuthorServices;
using RSS_Application.Context;
using Microsoft.AspNetCore.Identity;
using RSS_Application.Models.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using RSS_Application.Services.AuthServices;
using RSS_Application.Services.Initializer.Models;
using RSS_Application.Services.Initializer.Auth;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

IServiceCollection services = builder.Services;

string connectionStr = configuration.GetConnectionString("DefaultConnection")!;
services.AddDbContext<NewsContext>(opts => opts.UseSqlServer(connectionStr));

services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<NewsContext>();
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);

services.AddScoped<IPostService, PostService>();
services.AddScoped<IAuthorService, AuthorManager>();
services.AddScoped<IFeedService, FeedManager>();
services.AddScoped<ModelsInitializer, DefaultModelsInitializer>();
services.AddScoped<IAuthService, AuthManager>();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHsts();
}
else
    app.UseHsts();

app.UseHttpsRedirection();

app.UseRouting();

using (IServiceScope serviceScope = app.Services.CreateScope())
{
    IServiceProvider serviceProvider = serviceScope.ServiceProvider;

    var modelsInitializer = serviceProvider.GetRequiredService<ModelsInitializer>();
    await modelsInitializer.InitializeAsync();

    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RolesInitializer.InitializeAsync(roleManager);

    string rootAdmin = "Admin";
    string adminName = configuration.GetValue<string>($"{rootAdmin}:UserName");
    string adminPassword = configuration.GetValue<string>($"{rootAdmin}:Password");
    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    await UsersInitializer.AdminInitializeAsync(userManager, adminName, adminPassword);
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
