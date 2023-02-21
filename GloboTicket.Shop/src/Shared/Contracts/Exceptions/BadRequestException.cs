namespace GloboTicket.Shop.Shared.Contracts.Exceptions;

public sealed class BadRequestException : Exception
{
    /// <inheritdoc />
    public BadRequestException()
    {
    }

    /// <inheritdoc />
    public BadRequestException(string? message)
        : base(message)
    {
    }

    /// <inheritdoc />
    public BadRequestException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
