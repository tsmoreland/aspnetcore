﻿//
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

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace GloboTicket.TicketManagement.Identity.Models;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    /// <inheritdoc />
    [ProtectedPersonalData]
    public override string? UserName
    {
        get => base.UserName;
        set => base.UserName = value;
    }

    /// <inheritdoc />
    [ProtectedPersonalData]
    public override string? Email
    {
        get => base.Email;
        set => base.Email = value;
    }

    [MemberNotNullWhen(true, nameof(UserName))]
    [MemberNotNullWhen(true, nameof(Email))]
    public bool IsValid()
    {
        return UserName is { Length: > 0 } && Email is { Length: > 0 };
    }
}
