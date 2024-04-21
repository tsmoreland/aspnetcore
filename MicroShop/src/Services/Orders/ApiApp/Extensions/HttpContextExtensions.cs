//
// Copyright © 2023 Terry Moreland
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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MicroShop.Services.Orders.ApiApp.Extensions;

internal static class HttpContextExtensions
{
    public static bool TryGetUserIdFromHttpContext(this HttpContext context, [NotNullWhen(true)] out string? userId)
    {
        const string nameIdentiferClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        Claim? claim = context.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub) ?? context.User.Claims.FirstOrDefault(c => c.Type == nameIdentiferClaim);
        userId = claim?.Value;
        return userId is not null;
    }
}
