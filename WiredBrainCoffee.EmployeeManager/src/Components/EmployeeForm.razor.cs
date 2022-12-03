using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WiredBrainCoffee.EmployeeManager.Domain.DataTramsferObjects;
using WiredBrainCoffee.EmployeeManager.Domain.Models;

namespace WiredBrainCoffee.EmployeeManager.Components;

public partial class EmployeeForm
{
    [Parameter]
    public bool IsBusy { get; set; }

    [Parameter]
    public AddEmployeeDto? Employee { get; set; }

    [Parameter]
    public List<Department>? Departments { get; set; }

    [Parameter] 
    public EventCallback<bool> OnSubmit { get; set; }

    private async Task OnValidSubmit()
    {
        if (OnSubmit.HasDelegate)
        {
            await OnSubmit.InvokeAsync(true);
        }
    }

    private async Task OnInvalidSubmit()
    {
        if (OnSubmit.HasDelegate)
        {
            await OnSubmit.InvokeAsync(false);
        }
    }
}
