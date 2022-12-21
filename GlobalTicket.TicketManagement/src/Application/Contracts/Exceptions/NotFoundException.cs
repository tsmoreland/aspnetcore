using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GlobalTicket.TicketManagement.Application.Contracts.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string name, object key, Exception? innerException = null)
        : base($"{name} ({key} is not found", innerException)
    {
    }

    public static void ThrowIfNull<T, TKey>(T? @object, TKey key)
    {
        if (@object is null)
        {
            throw new NotFoundException(typeof(T).Name, key!);
        }
    }
}
