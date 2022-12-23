using FluentValidation.Results;
using GlobalTicket.TicketManagement.Application.Responses;

namespace GlobalTicket.TicketManagement.Application.Features.Categories.Commands.CreateCategory;

public sealed record class CreateCategoryCommandResponse(bool Success, CreateCategoryDto? Category, string? Message, IReadOnlyDictionary<string, string>? ValidationErrors)
    : BaseResponse(Success, Message, ValidationErrors)
{
    public CreateCategoryCommandResponse(CreateCategoryDto category)
        : this(true, category, null, null)
    {
    }

    public static CreateCategoryCommandResponse CreateFromValidationError(ValidationResult validationResult)
    {
        Dictionary<string, string> errorsByProperty = new();
        foreach (ValidationFailure error in validationResult.Errors)
        {
            (string property, string errorMessage) = (error.PropertyName, error.ErrorMessage);
            errorsByProperty[property] = errorMessage;
        }

        return new CreateCategoryCommandResponse(false, null, null, errorsByProperty.AsReadOnly());
    }
}
