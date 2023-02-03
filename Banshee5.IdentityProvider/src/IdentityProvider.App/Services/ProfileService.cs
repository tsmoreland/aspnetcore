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

using System.Security.Claims;
using Banshee5.IdentityProvider.App.Models;
using Duende.IdentityServer.AspNetIdentity;
using Microsoft.AspNetCore.Identity;

namespace Banshee5.IdentityProvider.App.Services;

public sealed class ProfileService : ProfileService<ApplicationUser>
{
    /// <inheritdoc />
    public ProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        : base(userManager, claimsFactory)
    {
    }

    /// <inheritdoc />
    public ProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, ILogger<ProfileService<ApplicationUser>> logger)
        : base(userManager, claimsFactory, logger)
    {
    }

    /// <inheritdoc />
    protected override async Task<ClaimsPrincipal> GetUserClaimsAsync(ApplicationUser user)
    {
        ClaimsPrincipal principal =  await base.GetUserClaimsAsync(user);
        if (principal.Identity is not ClaimsIdentity identity)
        {
            return principal;
        }

        Claim? existingClaim = identity.Claims.FirstOrDefault(c => c.Type == "location");
        if (existingClaim is not null)
        {
            return principal;
        }

        identity.AddClaim(new Claim("location", user.Location);

        return principal;
    }
}
