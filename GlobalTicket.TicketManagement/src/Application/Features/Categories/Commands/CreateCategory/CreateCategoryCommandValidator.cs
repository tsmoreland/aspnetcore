using FluentValidation;

namespace GlobalTicket.TicketManagement.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    /// <inheritdoc />
    public CreateCategoryCommandValidator()
    {
    }
}
