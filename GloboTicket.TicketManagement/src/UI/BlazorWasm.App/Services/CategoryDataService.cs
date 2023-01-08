using AutoMapper;
using Blazored.LocalStorage;
using GloboTicket.TicketManagement.UI.ApiClient.Contracts;
using GloboTicket.TicketManagement.UI.ApiClient.Services;
using GloboTicket.TicketManagement.UI.ApiClient.ViewModels;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Services;

public sealed class CategoryDataService : BaseDataService, ICategoryDataService
{
    public CategoryDataService(IClient client, IMapper mapper, ILocalStorageService localStorageService)
        : base(client, mapper, localStorageService)
    {
    }

    /// <inheritdoc />
    public async ValueTask<Page<CategoryViewModel>> GetCategoriesPage(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        CategoryWithEventsViewModelPage page = await Client.GetCategoriesAsync(false, pageNumber, pageSize, cancellationToken);
        return Mapper.Map<Page<CategoryViewModel>>(page);
    }

    /// <inheritdoc />
    public async ValueTask<Page<CategoryEventsViewModel>> GetCategoriesWithEventsPage(int pageNumber, int pageSize, bool includeHistory, CancellationToken cancellationToken)
    {
        CategoryWithEventsViewModelPage page = await Client.GetCategoriesAsync(includeHistory, pageNumber, pageSize, cancellationToken);
        return Mapper.Map<Page<CategoryEventsViewModel>>(page);
    }

    /// <inheritdoc />
    public async ValueTask<ApiResponse<CreateCategoryDto>> CreateCategory(CategoryViewModel categoryViewModel, CancellationToken cancellationToken)
    {
        try
        {
            CreateCategoryCommand command = Mapper.Map<CreateCategoryCommand>(categoryViewModel);
            CreateCategoryCommandResponse response = await Client.AddCategoryAsync(command, cancellationToken);
            return Mapper.Map<ApiResponse<CreateCategoryDto>>(response);
        }
        catch (ApiException ex)
        {
            return ConvertApiException<CreateCategoryDto>(ex);
        }
    }
}
