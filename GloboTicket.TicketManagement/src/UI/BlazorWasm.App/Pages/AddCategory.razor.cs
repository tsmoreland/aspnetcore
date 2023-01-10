using System.Text;
using GloboTicket.TicketManagement.UI.ApiClient.Contracts;
using GloboTicket.TicketManagement.UI.ApiClient.Services;
using GloboTicket.TicketManagement.UI.ApiClient.ViewModels;
using Microsoft.AspNetCore.Components;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Pages;

public partial class AddCategory
{
    [Inject]
    public ICategoryDataService CategoryDataService { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    public CategoryViewModel CategoryViewModel { get; set; } = default!;

    public string? Message { get; set; } 


    protected override void OnInitialized()
    {
        CategoryViewModel = new CategoryViewModel();
    }

    protected async Task HandleValidSubmit()
    {
        ApiResponse<CreateCategoryDto> response = await CategoryDataService.CreateCategory(CategoryViewModel, default);
        HandleResponse(response);
    }

    private void HandleResponse(ApiResponse<CreateCategoryDto> response)
    {
        if (response.Success)
        {
            Message = "Category added";
        }
        else
        {
            StringBuilder builder = new();
            builder.Append(response.Message);
            if (response.ProblemDetails != null)
            {
                foreach (string problem in response.ProblemDetails.Values)
                {
                    builder.Append(problem);
                }
            }
            Message = builder.ToString();
        }
    }
}
