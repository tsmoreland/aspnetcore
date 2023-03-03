﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable disable

namespace Banshee5.IdentityProvider.App.Data.CompiledModels
{
    public partial class ApplicationDbContextModel
    {
        partial void Initialize()
        {
            var applicationUser = ApplicationUserEntityType.Create(this);
            var identityRole = IdentityRoleEntityType.Create(this);
            var identityRoleClaimstring = IdentityRoleClaimstringEntityType.Create(this);
            var identityUserClaimstring = IdentityUserClaimstringEntityType.Create(this);
            var identityUserLoginstring = IdentityUserLoginstringEntityType.Create(this);
            var identityUserRolestring = IdentityUserRolestringEntityType.Create(this);
            var identityUserTokenstring = IdentityUserTokenstringEntityType.Create(this);

            IdentityRoleClaimstringEntityType.CreateForeignKey1(identityRoleClaimstring, identityRole);
            IdentityUserClaimstringEntityType.CreateForeignKey1(identityUserClaimstring, applicationUser);
            IdentityUserLoginstringEntityType.CreateForeignKey1(identityUserLoginstring, applicationUser);
            IdentityUserRolestringEntityType.CreateForeignKey1(identityUserRolestring, identityRole);
            IdentityUserRolestringEntityType.CreateForeignKey2(identityUserRolestring, applicationUser);
            IdentityUserTokenstringEntityType.CreateForeignKey1(identityUserTokenstring, applicationUser);

            ApplicationUserEntityType.CreateAnnotations(applicationUser);
            IdentityRoleEntityType.CreateAnnotations(identityRole);
            IdentityRoleClaimstringEntityType.CreateAnnotations(identityRoleClaimstring);
            IdentityUserClaimstringEntityType.CreateAnnotations(identityUserClaimstring);
            IdentityUserLoginstringEntityType.CreateAnnotations(identityUserLoginstring);
            IdentityUserRolestringEntityType.CreateAnnotations(identityUserRolestring);
            IdentityUserTokenstringEntityType.CreateAnnotations(identityUserTokenstring);

            AddAnnotation("ProductVersion", "6.0.0");
        }
    }
}
