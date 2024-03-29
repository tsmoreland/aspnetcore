﻿using GloboTicket.TicketManagement.UI.ApiClient.Services;
using GloboTicket.TicketManagement.UI.ApiClient.ViewModels;

namespace GloboTicket.TicketManagement.UI.ApiClient.Contracts;

public interface ICategoryDataService
{
    // TODO: use NSWAG to generate client side code from Swagger endpoint when running in debug
    ValueTask<Page<CategoryViewModel>> GetCategoriesPage(int pageNumber, int pageSize, CancellationToken cancellationToken);
    ValueTask<Page<CategoryEventsViewModel>> GetCategoriesWithEventsPage(int pageNumber, int pageSize, bool includeHistory, CancellationToken cancellationToken);
    ValueTask<ApiResponse<CreateCategoryDto>> CreateCategory(CategoryViewModel categoryViewModel, CancellationToken cancellationToken);
}
