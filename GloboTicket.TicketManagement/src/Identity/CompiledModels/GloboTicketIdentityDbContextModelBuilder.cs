﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable enable

namespace GloboTicket.TicketManagement.Identity.CompiledModels
{
    public partial class GloboTicketIdentityDbContextModel
    {
        partial void Initialize()
        {
            var applicationRole = ApplicationRoleEntityType.Create(this);
            var applicationUser = ApplicationUserEntityType.Create(this);
            var identityRoleClaim = IdentityRoleClaimEntityType.Create(this);
            var identityUserClaim = IdentityUserClaimEntityType.Create(this);
            var identityUserLogin = IdentityUserLoginEntityType.Create(this);
            var identityUserRole = IdentityUserRoleEntityType.Create(this);
            var identityUserToken = IdentityUserTokenEntityType.Create(this);

            IdentityRoleClaimEntityType.CreateForeignKey1(identityRoleClaim, applicationRole);
            IdentityUserClaimEntityType.CreateForeignKey1(identityUserClaim, applicationUser);
            IdentityUserLoginEntityType.CreateForeignKey1(identityUserLogin, applicationUser);
            IdentityUserRoleEntityType.CreateForeignKey1(identityUserRole, applicationRole);
            IdentityUserRoleEntityType.CreateForeignKey2(identityUserRole, applicationUser);
            IdentityUserTokenEntityType.CreateForeignKey1(identityUserToken, applicationUser);

            ApplicationRoleEntityType.CreateAnnotations(applicationRole);
            ApplicationUserEntityType.CreateAnnotations(applicationUser);
            IdentityRoleClaimEntityType.CreateAnnotations(identityRoleClaim);
            IdentityUserClaimEntityType.CreateAnnotations(identityUserClaim);
            IdentityUserLoginEntityType.CreateAnnotations(identityUserLogin);
            IdentityUserRoleEntityType.CreateAnnotations(identityUserRole);
            IdentityUserTokenEntityType.CreateAnnotations(identityUserToken);

            AddAnnotation("ProductVersion", "7.0.1");
        }
    }
}
