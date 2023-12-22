using BethanysPieShopHRM.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BethanysPieShopHRM.Web.Components;

public partial class Map
{
    public string ElementId { get; } = $"map-{Guid.NewGuid():D}";

    [Parameter]
    public double Zoom { get; set; }

    [Parameter]
    public List<Marker>? Markers { get; set; } = [];

    [Inject]
    public IJSRuntime JsRuntime { get; set; } = null!;


    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        List<Marker> markers = Markers ?? [];

        await JsRuntime.InvokeVoidAsync("deliveryMap.showOrUpdate", ElementId, markers);
    }
}
