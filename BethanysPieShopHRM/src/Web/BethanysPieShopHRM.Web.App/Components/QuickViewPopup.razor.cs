using BethanysPieShopHRM.Shared.Domain;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.App.Components;

public partial class QuickViewPopup
{
    private Employee? _employee;

    [Parameter]
    public Employee Employee { get; set; } = default!;

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        _employee = Employee;
    }

    public void Close()
    {
        _employee = null;
    }
}
