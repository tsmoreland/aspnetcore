using FluentValidation.Results;

namespace GloboTicket.TicketManagement.Domain.Contracts.Exceptions;

public sealed class ValidationFailureException : Exception
{
    /// <inheritdoc />
    public ValidationFailureException(ValidationResult validationResult)
    {
        Dictionary<string, string> errorsByProperty = new();
        foreach (ValidationFailure error in validationResult.Errors)
        {
            errorsByProperty[error.PropertyName] = error.ErrorMessage;
        }

        ValidationErrors = errorsByProperty.AsReadOnly();
    }

    public IReadOnlyDictionary<string, string> ValidationErrors { get; }

    public static void ThrowIfHasErrors(ValidationResult validationResult)
    {
        if (validationResult.Errors.Count > 0)
        {
            throw new ValidationFailureException(validationResult);
        }
    }
}
