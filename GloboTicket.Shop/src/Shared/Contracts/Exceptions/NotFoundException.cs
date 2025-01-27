namespace GloboTicket.Shop.Shared.Contracts.Exceptions;

#pragma warning disable IDE0079
#pragma warning disable RCS1222
#pragma warning disable RCS1194
public sealed class NotFoundException(string name, object key, Exception? innerException = null)
    : Exception($"{name} ({key} is not found", innerException)
{
    public NotFoundException(string name, string key) : this(name, key, null)
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
#pragma warning restore IDE0079, RCS1222, RCS1194
