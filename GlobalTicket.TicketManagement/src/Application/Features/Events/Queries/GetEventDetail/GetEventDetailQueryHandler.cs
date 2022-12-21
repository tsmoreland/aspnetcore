using AutoMapper;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GlobalTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;

public sealed class GetEventDetailQueryHandler : IRequestHandler<GetEventDetailQuery, EventDetailViewModel?>
{
    private readonly IMapper _mapper;
    private readonly IAsyncRepository<Event> _eventRepository;
    private readonly IAsyncRepository<Category> _categoryRepository;

    public GetEventDetailQueryHandler(IMapper mapper, IAsyncRepository<Event> eventRepository, IAsyncRepository<Category> categoryRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <inheritdoc />
    public async Task<EventDetailViewModel?> Handle(GetEventDetailQuery request, CancellationToken cancellationToken)
    {
        // not really an applciation layer concern but would rather an extra parameter here to specify to what children to include
        // - eventual plan -
        // add a builder factory - builder can be used to setup FilterSpec, OrderBySpec and IncludeSpec allowing event repo to get eveything in one request
        // and a final selector spec can be used to project to the view model negating the need for AutoMapper - more for the efficiency of the query than need
        // to avoid AutoMapper
        Event? @event = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);
        if (@event is null)
        {
            return null;
        }
        EventDetailViewModel eventVm = _mapper.Map<EventDetailViewModel>(@event);

        Category? category = await _categoryRepository.GetByIdAsync(eventVm.CategoryId, cancellationToken);
        if (category is null)
        {
            return null;
        }

        eventVm.Category = _mapper.Map<CategoryDto>(category);
        return eventVm;
    }
}
