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
using Banshee5.IdentityProvider.Data;
using Banshee5.IdentityProvider.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Banshee5.IdentityProvider;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (IServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            ApplicationDbContext context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            UserManager<ApplicationUser> userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            ApplicationUser diana = userMgr.FindByNameAsync("Diana").Result;
            if (diana == null)
            {
                diana = new ApplicationUser
                {
                    UserName = "dprince",
                    Email = "diana.princejusticeleague.com",
                    EmailConfirmed = true,
                };
                IdentityResult result = userMgr.CreateAsync(diana, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(diana, new Claim[]{
                    new(JwtClaimTypes.Name, "Diana Prince"),
                    new(JwtClaimTypes.GivenName, "Diana"),
                    new(JwtClaimTypes.FamilyName, "Prince"),
                    new("location", "Themyscira ")
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("Diana created");
            }
            else
            {
                Log.Debug("Diana already exists");
            }

            ApplicationUser bruce = userMgr.FindByNameAsync("Bruce").Result;
            if (bruce == null)
            {
                bruce = new ApplicationUser
                {
                    UserName = "bwayne",
                    Email = "bruce.wayne@wayne-enterprises.com",
                    EmailConfirmed = true
                };
                IdentityResult result = userMgr.CreateAsync(bruce, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(bruce, new Claim[]{
                    new(JwtClaimTypes.Name, "Bruce Wayne"),
                    new(JwtClaimTypes.GivenName, "Bruce"),
                    new(JwtClaimTypes.FamilyName, "Wayne"),
                    new("location", "Gotham")
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("Bruce created");
            }
            else
            {
                Log.Debug("Bruce already exists");
            }
        }
    }
}