using AutoMapper;
using GlobalTicket.TicketManagement.Application.Features.Events;
using GlobalTicket.TicketManagement.Domain.Entities;

namespace GlobalTicket.TicketManagement.Application.Profiles;

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
    }
    private void CreateEventMaps()
    {
        CreateMap<Event, EventViewModel>().ReverseMap();
        CreateMap<Event, EventDetailViewModel>().ReverseMap();
    }
    private void CreateOrderMaps()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}
