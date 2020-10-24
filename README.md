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
