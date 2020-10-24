# Blazor Server Identity Provider with ASP.NET Core Identity using OpenIddict

Based on [official microsoft documentation for ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-3.1&tabs=netcore-cli#scaffold-identity-into-a-blazor-server-project-with-authorization).

In this sample we will be using:

- Blazor Server
- ASP.NET Core Identity
- SQLite
- [OpenIddict Core](https://github.com/openiddict/openiddict-core)
- [OpenIddict Samples](https://github.com/openiddict/openiddict-samples)

## Steps to run through this sample/tutorial

### Create the solution file

```powershell
dotnet new sln -n BlazorIdentity
```

### Create and add a clean blazor server project

```powershell
dotnet new blazorserver -n BlazorIdentity.Server
dotnet sln add BlazorIdentity.Server
```

### Add dependencies to the project

Install the tools used in this sample:

```powershell
dotnet tool install -g dotnet-ef
dotnet tool install -g dotnet-aspnet-codegenerator
```

Add the packages:

```powershell
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package OpenIddict.AspNetCore -v 3.0.0-beta5.20503.76
dotnet add package OpenIddict.EntityFrameworkCore -v 3.0.0-beta5.20503.76
```

### Create the Database context

Create an ApplicationUser class in /Models:

```csharp
public class ApplicationUser : IdentityUser
{
}
```

Create the DbContext class in /Data:

```csharp
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
```

Create an initial migration and run it:

```powershell
dotnet ef migrations add InitialSchema -o "Data/Migrations"
dotnet ef database update
```

### Setup other required classes before scaffolding

Create /Services/EmailSender.cs:

```csharp
public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Task.CompletedTask;
    }
}
```

Add to Startup.cs, ConfigureServices method:

```csharp
services.AddDbContext<AppDbContext>(options =>
{
    // Configure the context to use SQLite.
    options.UseSqlite(Configuration.GetConnectionString("AppDbContextConnection"));

    // Register the entity sets needed by OpenIddict.
    // Note: use the generic overload if you need
    // to replace the default OpenIddict entities.
    options.UseOpenIddict();
});

services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    // Add more password requirements ...
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configure Identity to use the same JWT claims as OpenIddict instead
// of the legacy WS-Federation claims it uses by default (ClaimTypes),
// which saves you from doing the mapping in your authorization controller.
services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserNameClaimType = Claims.Name;
    options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
    options.ClaimsIdentity.RoleClaimType = Claims.Role;
});

services.AddOpenIddict()
    // Register the OpenIddict core components.
    .AddCore(options =>
    {
        // Configure OpenIddict to use the Entity Framework Core stores and models.
        // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
        options.UseEntityFrameworkCore()
                .UseDbContext<AppDbContext>();
    })

    // Register the OpenIddict server components.
    .AddServer(options =>
    {
        // Enable the authorization, logout, token and userinfo endpoints.
        options.SetAuthorizationEndpointUris("/connect/authorize")
                .SetLogoutEndpointUris("/connect/logout")
                .SetTokenEndpointUris("/connect/token")
                .SetUserinfoEndpointUris("/connect/userinfo");

        // Mark the "email", "profile" and "roles" scopes as supported scopes.
        options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

        // Note: the sample uses the code and refresh token flows but you can enable
        // the other flows if you need to support implicit, password or client credentials.
        options.AllowAuthorizationCodeFlow()
                .AllowRefreshTokenFlow();

        // Register the signing and encryption credentials.
        options.AddDevelopmentEncryptionCertificate()
                .AddDevelopmentSigningCertificate();

        // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
        options.UseAspNetCore()
                .EnableAuthorizationEndpointPassthrough()
                .EnableLogoutEndpointPassthrough()
                .EnableStatusCodePagesIntegration()
                .EnableTokenEndpointPassthrough();
    })

    // Register the OpenIddict validation components.
    .AddValidation(options =>
    {
        // Import the configuration from the local OpenIddict server instance.
        options.UseLocalServer();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();
    });

// Add our custom Email Sender
services.AddSingleton<IEmailSender, EmailSender>();
```

Add to Startup.cs, Configure method:

```csharp
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();

    endpoints.MapBlazorHub(); // Enable For Blazor Server only
    endpoints.MapFallbackToPage("/_Host"); // Enable For Blazor Server only
});
```

### Scaffold all the Identity files

```powershell
dotnet aspnet-codegenerator identity -dc BlazorIdentity.Server.Data.AppDbContext -sqlite --files "Account.Register;Account.Login;Account.Logout;Account.ResetPassword"
```
