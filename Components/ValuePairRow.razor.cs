using Microsoft.AspNetCore.Components;
using SLocker.Models;

namespace SLocker.Components;

public partial class ValuePairRow : ComponentBase
{
    [Parameter] public Pair Item { get; set; } = null!;

    private bool _visible = false;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
    }

    private void Delete() { }
}
