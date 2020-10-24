# Create the solution file
dotnet new sln -n BlazorIdentity

# Create and add a clean blazor server project
dotnet new blazorserver -n BlazorIdentity.Server
dotnet sln add BlazorIdentity.Server

# Step 1 complete!
# Commit to source control!