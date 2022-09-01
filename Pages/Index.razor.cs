using Microsoft.AspNetCore.Components;
using SLocker.Services;

namespace SLocker.Pages;

public partial class Index : ComponentBase
{
    [Inject] SecretService SS { get; set; } = null!;
    [Inject] NavigationManager Navigation { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }

    private string? _secret;

    private void SetSecret()
    {
        if (!string.IsNullOrEmpty(_secret))
        {
            SS.Secret = _secret;
            Navigation.NavigateTo("/dashboard");
        }
    }
}