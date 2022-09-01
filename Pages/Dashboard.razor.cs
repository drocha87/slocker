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
}
// <script setup lang="ts">
// import { useCounterStore } from "@/stores/counter";
// import { computed, onMounted, ref } from "vue";
// import LayoutStack from "../components/LayoutStack.vue";

// import ValuePairRow from "../components/ValuePairRow.vue";

// const store = useCounterStore();

// onMounted(async () => {
//   await store.fetchPairs();
// });

// async function updatePairs() {
//   await store.fetchPairs();
// }

// const pairs = computed(() => {
//   if (filter.value.length > 0) {
//     const regex = new RegExp(filter.value, "i");
//     return store.pairs.filter((x) => regex.test(x.key));
//   }
//   return store.pairs;
// });

// async function addPair() {
//   const result = await fetch("http://localhost:5087/pairs", {
//     method: "POST",
//     headers: {
//       "content-type": "application/json",
//     },
//     body: JSON.stringify({
//       key: key.value,
//       value: value.value,
//     }),
//   });

//   if (result.ok) {
//     key.value = "";
//     value.value = "";
//     await store.fetchPairs();
//   }

//   return true;
// }

// function cancel() {
//   showNewApplicationSettingsModal.value = false;
//   key.value = "";
//   value.value = "";
// }

// const key = ref("");
// const filter = ref("");
// const value = ref("");
// const showNewApplicationSettingsModal = ref(false);
// </script>