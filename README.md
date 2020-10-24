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

Install the tools:

```powershell
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

### Scaffold all the Identity files

```powershell
dotnet aspnet-codegenerator identity -dc BlazorIdentity.Server.Data.AppDbContext -sqlite --files "Account.Register;Account.Login;Account.Logout;Account.ResetPassword"
```
