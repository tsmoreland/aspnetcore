﻿using AutoMapper;
using GlobalTicket.TicketManagement.Application.Features.Categories.Queries.GetCatagoriesPage;
using GlobalTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesPageWithEvents;
using GlobalTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
using GlobalTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;
using GlobalTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;
using GlobalTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;
using GlobalTicket.TicketManagement.Application.Features.Events.Queries.GetEventsPage;
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