//
// Copyright © 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using AutoMapper;
using GloboTicket.TicketManagement.UI.ApiClient.Services;
using GloboTicket.TicketManagement.UI.ApiClient.ViewModels;
using EventDetailViewModel = GloboTicket.TicketManagement.UI.ApiClient.ViewModels.EventDetailViewModel;

namespace GloboTicket.TicketManagement.UI.ApiClient.Profiles;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        ConfigureEventMappings();
        ConfigureCategoryMappings();
        ConfigureOrderMappings();
    }

    public void ConfigureEventMappings()
    {
        CreateMap<EventListViewModel, EventViewModel>().ReverseMap();
        CreateMap<EventViewModelPage, Page<EventListViewModel>>()
            .ConstructUsing((m, ctx) => m.Items == null
                ? new Page<EventListViewModel>(m.PageNumber ?? 0, m.PageSize ?? 0, 0, 0, Array.Empty<EventListViewModel>())
                : new Page<EventListViewModel>(m.PageNumber ?? 0, m.PageSize ?? 0, m.TotalPages ?? 0, m.TotalSize ?? 0, ctx.Mapper.Map<List<EventListViewModel>>(m.Items)));
        CreateMap<Services.EventDetailViewModel, EventViewModel?>();

        CreateMap<EventDetailViewModel, CreateEventCommand>();
        CreateMap<EventDetailViewModel, UpdateEventDto>();

    }
    public void ConfigureCategoryMappings()
    {
        CreateMap<CategoryWithEventsViewModel, CategoryEventViewModel>().ReverseMap();
        CreateMap<CategoryWithEventsViewModel, CategoryViewModel>().ReverseMap();

        CreateMap<CategoryWithEventsViewModelPage, Page<CategoryEventsViewModel>>()
            .ConstructUsing((m, ctx) => m.Items == null
                ? new Page<CategoryEventsViewModel>(m.PageNumber ?? 0, m.PageSize ?? 0, 0, 0, Array.Empty<CategoryEventsViewModel>())
                : new Page<CategoryEventsViewModel>(m.PageNumber ?? 0, m.PageSize ?? 0, m.TotalPages ?? 0, m.TotalSize ?? 0, ctx.Mapper.Map<List<CategoryEventsViewModel>>(m.Items)));

        CreateMap<CategoryWithEventsViewModelPage, Page<CategoryViewModel>>()
            .ConstructUsing((m, ctx) => m.Items == null
                ? new Page<CategoryViewModel>(m.PageNumber ?? 0, m.PageSize ?? 0, 0, 0, Array.Empty<CategoryViewModel>())
                : new Page<CategoryViewModel>(m.PageNumber ?? 0, m.PageSize ?? 0, m.TotalPages ?? 0, m.TotalSize ?? 0, ctx.Mapper.Map<List<CategoryViewModel>>(m.Items)));


        CreateMap<CreateCategoryDto, CategoryViewModel>().ReverseMap();
        CreateMap<CreateCategoryCommandResponse, ApiResponse<CategoryViewModel>>()
            .ConstructUsing((m, ctx) => m.Success == true
                ? ApiResponse.Success(ctx.Mapper.Map<CategoryViewModel>(m.Category))
                : ApiResponse.FromValidationErrors<CategoryViewModel>(m.Message ?? "An unknown error occurred.", m.ValidationErrors));

    }
    public void ConfigureOrderMappings()
    {
    }
}
