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
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.Certificate;
using SunDoeCoffeeShop.FrontEnd.App.Application.Contracts.Authentication;
using SunDoeCoffeeShop.Shared.Roles;

namespace SunDoeCoffeeShop.FrontEnd.App.Application.Features.Authentication.Commands.CertificateValidated;

public class CertificateValidatedHandler : ICertificateValidatedHandler
{
    private readonly ILogger<CertificateValidatedHandler> _logger;

    public CertificateValidatedHandler(ILogger<CertificateValidatedHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public Task Handle(CertificateValidatedContext context)
    {
        if (!IsCertificateAuthorized(context.ClientCertificate))
        {
            context.Fail(new UnauthorizedAccessException());
            return Task.CompletedTask;
        }


        string name = context.ClientCertificate.FriendlyName;
        if (name is not { Length: > 0 })
        {
            name = context.ClientCertificate.SubjectName.Name.Replace("CN=", "");
        }

        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, context.ClientCertificate.Thumbprint, ClaimValueTypes.String, context.Options.ClaimsIssuer),
            new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, context.Options.ClaimsIssuer),
            new Claim(ClaimTypes.Role, Role.Administrator.ToString(), ClaimValueTypes.String, context.Options.ClaimsIssuer),
        };

        context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
        context.Success();
        return Task.CompletedTask;
    }

    private bool IsCertificateAuthorized(X509Certificate2 certificate)
    {
        _ = certificate;
        return true;
    }
}
