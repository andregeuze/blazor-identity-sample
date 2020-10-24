using Microsoft.AspNetCore.Components;
using System;

namespace BlazorIdentity.Server.Shared
{
    public class RedirectToLogin : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo($"authentication/login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
        }
    }

}
