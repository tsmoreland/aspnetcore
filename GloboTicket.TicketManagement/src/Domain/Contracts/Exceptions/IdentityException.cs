//
// Copyright (c) 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using GloboTicket.TicketManagement.Domain.Models.Authentication;

namespace GloboTicket.TicketManagement.Domain.Contracts.Exceptions;

public sealed class IdentityException : Exception
{
    public IdentityError Error { get; }
    public IReadOnlyDictionary<string, string> ValidationErrors { get; }

    /// <inheritdoc />
    public IdentityException(IdentityError error)
        : this(error, null, null)
    {
        ValidationErrors = new Dictionary<string, string>();
    }

    public IdentityException(IdentityError error, IReadOnlyDictionary<string, string> validationErrors)
        : this(error, null, null)
    {
        ValidationErrors = validationErrors.ToDictionary(p => p.Key, p => p.Value).AsReadOnly();
    }

    /// <inheritdoc />
    public IdentityException(IdentityError error, string? message)
        : this(error, message, null)
    {
        ValidationErrors = new Dictionary<string, string>();
    }

    /// <inheritdoc />
    public IdentityException(IdentityError error, string? message, Exception? innerException)
        : base(message, innerException)
    {
        Error = error;
        ValidationErrors = new Dictionary<string, string>();
    }

    public string? GetUserFriendlyDetail()
    {
        return Error switch
        {
            IdentityError.UserAlreadyExists => "username or password is incorrect.",
            IdentityError.UserNotFound => // treating not found and already exists as unhelpful to reduce risk of brute force
                "username or password is incorrect.",
            IdentityError.InvalidCredentials => "username or password is incorrect.",
            IdentityError.ValidationError => Message,
            _ => null
        };
    }
}
