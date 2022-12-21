using FluentValidation.Results;

namespace GlobalTicket.TicketManagement.Application.Contracts.Exceptions;

public sealed class ValidationFailureException : Exception
{
    /// <inheritdoc />
    public ValidationFailureException(ValidationResult validationResult)
    {
        ValidationErrors = validationResult.Errors
            .Select(validationError => validationError.ErrorMessage)
            .ToList()
            .AsReadOnly();
    }

    public IReadOnlyList<string> ValidationErrors { get; }

    public static void ThrowIfHasErrors(ValidationResult validationResult)
    {
        if (validationResult.Errors.Count > 0)
        {
            throw new ValidationFailureException(validationResult);
        }
    }
}
