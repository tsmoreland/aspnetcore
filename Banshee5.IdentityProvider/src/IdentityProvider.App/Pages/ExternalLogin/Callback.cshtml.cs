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
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Banshee5.IdentityProvider.App.Pages.ExternalLogin;

[AllowAnonymous]
[SecurityHeaders]
public class Callback : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ILogger<Callback> _logger;
    private readonly IEventService _events;

    public Callback(
        IIdentityServerInteractionService interaction,
        IEventService events,
        ILogger<Callback> logger,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _logger = logger;
        _events = events;
    }

    public async Task<IActionResult> OnGet()
    {
        // read external identity from the temporary cookie
        AuthenticateResult result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
        if (result?.Succeeded != true)
        {
            throw new Exception("External authentication error");
        }

        ClaimsPrincipal externalUser = result.Principal;

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            IEnumerable<string> externalClaims = externalUser.Claims.Select(c => $"{c.Type}: {c.Value}");
            _logger.LogDebug("External claims: {@claims}", externalClaims);
        }

        // lookup our user and external provider info
        // try to determine the unique id of the external user (issued by the provider)
        // the most common claim type for that are the sub claim and the NameIdentifier
        // depending on the external provider, some other claim type might be used
        Claim userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                            externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                            throw new Exception("Unknown userid");

        string provider = result.Properties.Items["scheme"];
        string providerUserId = userIdClaim.Value;

        // find external user
        ApplicationUser user = await _userManager.FindByLoginAsync(provider, providerUserId);
        if (user == null)
        {
            // this might be where you might initiate a custom workflow for user registration
            // in this sample we don't show how that would be done, as our sample implementation
            // simply auto-provisions new external user
            user = await AutoProvisionUserAsync(provider, providerUserId, externalUser.Claims);
        }

        // this allows us to collect any additional claims or properties
        // for the specific protocols used and store them in the local auth cookie.
        // this is typically used to store data needed for signout from those protocols.
        List<Claim> additionalLocalClaims = new List<Claim>();
        AuthenticationProperties localSignInProps = new AuthenticationProperties();
        CaptureExternalLoginContext(result, additionalLocalClaims, localSignInProps);

        // issue authentication cookie for user
        await _signInManager.SignInWithClaimsAsync(user, localSignInProps, additionalLocalClaims);

        // delete temporary cookie used during external authentication
        await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        // retrieve return URL
        string returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

        // check if external login is in the context of an OIDC request
        AuthorizationRequest context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id, user.UserName, true, context?.Client.ClientId));

        if (context != null)
        {
            if (context.IsNativeClient())
            {
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                return this.LoadingPage(returnUrl);
            }
        }

        return Redirect(returnUrl);
    }

    private async Task<ApplicationUser> AutoProvisionUserAsync(string provider, string providerUserId, IEnumerable<Claim> claims)
    {
        string sub = Guid.NewGuid().ToString();

        ApplicationUser user = new ApplicationUser
        {
            Id = sub,
            UserName = sub, // don't need a username, since the user will be using an external provider to login
        };

        // email
        string email = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value ??
                       claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (email != null)
        {
            user.Email = email;
        }

        // create a list of claims that we want to transfer into our store
        List<Claim> filtered = new List<Claim>();

        // user's display name
        string name = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value ??
                      claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        if (name != null)
        {
            filtered.Add(new Claim(JwtClaimTypes.Name, name));
        }
        else
        {
            string first = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value ??
                           claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            string last = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value ??
                          claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
            if (first != null && last != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
            }
            else if (first != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, first));
            }
            else if (last != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, last));
            }
        }

        IdentityResult identityResult = await _userManager.CreateAsync(user);
        if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

        if (filtered.Any())
        {
            identityResult = await _userManager.AddClaimsAsync(user, filtered);
            if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
        }

        identityResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));
        if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

        return user;
    }

    // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
    // this will be different for WS-Fed, SAML2p or other protocols
    private void CaptureExternalLoginContext(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
    {
        // capture the idp used to login, so the session knows where the user came from
        localClaims.Add(new Claim(JwtClaimTypes.IdentityProvider, externalResult.Properties.Items["scheme"]));

        // if the external system sent a session id claim, copy it over
        // so we can use it for single sign-out
        Claim sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
        if (sid != null)
        {
            localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
        }

        // if the external provider issued an id_token, we'll keep it for signout
        string idToken = externalResult.Properties.GetTokenValue("id_token");
        if (idToken != null)
        {
            localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
        }
    }
}
