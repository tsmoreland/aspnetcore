using FluentValidation.Results;

namespace GloboTicket.Shop.Shared.Contracts.Exceptions;

#pragma warning disable IDE0079
#pragma warning disable RCS1222
#pragma warning disable RCS1194
public sealed class ValidationFailureException : Exception
{
    /// <inheritdoc />
    public ValidationFailureException(ValidationResult validationResult)
    {
        Dictionary<string, string> errorsByProperty = [];
        foreach (var error in validationResult.Errors)
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
#pragma warning restore IDE0079, RCS1222, RCS1194
