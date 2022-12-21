using FluentValidation.Results;
using GlobalTicket.TicketManagement.Application.Responses;

namespace GlobalTicket.TicketManagement.Application.Features.Categories.Commands.CreateCategory;

public sealed record class CreateCategoryCommandResponse(bool Success, CreateCategoryDto? Category, string? Message, IReadOnlyList<string>? ValidationErrors)
    : BaseResponse(Success, Message, ValidationErrors)
{
    public CreateCategoryCommandResponse(CreateCategoryDto category)
        : this(true, category, null, null)
    {
    }

    public static CreateCategoryCommandResponse CreateFromValidationError(ValidationResult validationResult)
    {
        return new CreateCategoryCommandResponse(false, null, null, validationResult.Errors.Select(e => e.ErrorMessage).ToList().AsReadOnly());
    }
}
