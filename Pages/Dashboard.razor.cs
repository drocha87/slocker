using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SLocker.Models;
using SLocker.Services;

namespace SLocker.Pages;

public partial class Dashboard : ComponentBase
{
    [Inject] SecretService SS { get; set; } = null!;
    [Inject] NavigationManager Navigation { get; set; } = null!;
    [Inject] IJSRuntime JS { get; set; } = null!;

    private bool showNewApplicationSettingsModal = false;

    private List<Pair>? Pairs;

    private IJSObjectReference? _module;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var reference = DotNetObjectReference.Create(this);

            _module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/indexeddbWrapper.js");
            await _module.InvokeVoidAsync("initializeDatabase", reference, "slockerdb");
        }
    }

    protected override void OnInitialized()
    {
        if (!SS.HasSecret())
        {
            Navigation.NavigateTo("/");
        }
    }

    private string? _filter;

    public List<Pair>? FilteredPairs
    {
        get
        {
            var regex = new Regex(_filter ?? ".*");
            return Pairs?.Where(x => regex.IsMatch(x.Key)).ToList();
        }
    }

    private string? _pairName;
    private string? _pairValue;

    public async Task SavePair()
    {
        if (_module is not null)
        {
            if (!string.IsNullOrEmpty(_pairName) && !string.IsNullOrEmpty(_pairValue))
            {
                await _module.InvokeVoidAsync("putPair", _pairName, _pairValue);
            }
        }
    }

    public async Task DeletePair(Pair pair)
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("deletePair", pair.Id);
        }
    }

    [JSInvokable]
    public async Task DatabaseInitialized()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("getAllPairs");
        }
    }

    [JSInvokable]
    public async Task UpdatePairs(Pair[] pairs)
    {
        Pairs = pairs.ToList();
        await InvokeAsync(StateHasChanged);
    }

    [JSInvokable]
    public async Task PairSaved()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("getAllPairs");
        }
    }

    [JSInvokable]
    public async Task PairDeleted(double id)
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("getAllPairs");
        }
    }
}