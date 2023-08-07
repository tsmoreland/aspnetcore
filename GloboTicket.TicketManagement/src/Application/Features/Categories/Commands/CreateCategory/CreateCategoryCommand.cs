using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCategory;

public sealed record class CreateCategoryCommand(string Name) : IRequest<CreateCategoryCommandResponse>;
