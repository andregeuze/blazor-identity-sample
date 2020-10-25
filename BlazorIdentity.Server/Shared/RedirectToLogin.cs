using Microsoft.AspNetCore.Components;
using System;

namespace BlazorIdentity.Server.Shared
{
    public class RedirectToLogin : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo($"Identity/Account/Login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}", true);
        }
    }
}
