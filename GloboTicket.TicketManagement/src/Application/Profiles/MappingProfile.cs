using AutoMapper;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCatagoriesPage;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesPageWithEvents;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsPage;
using GloboTicket.TicketManagement.Domain.Entities;

namespace GloboTicket.TicketManagement.Application.Profiles;

public sealed class MappingProfile : Profile
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
        CreateCategoryMaps();
        CreateEventMaps();
        CreateOrderMaps();
    }

    private void CreateCategoryMaps()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<Category, CategoryViewModel>();
        CreateMap<Category, CategoryWithEventsViewModel>();
    }
    private void CreateEventMaps()
    {
        CreateMap<Event, EventViewModel>().ReverseMap();
        CreateMap<Event, EventDetailViewModel>().ReverseMap();

        CreateMap<Event, CreateEventCommand>().ReverseMap();
        CreateMap<Event, UpdateEventCommand>().ReverseMap();
        CreateMap<Event, DeleteEventCommand>().ReverseMap();
    }
    private void CreateOrderMaps()
    {
    }
}
