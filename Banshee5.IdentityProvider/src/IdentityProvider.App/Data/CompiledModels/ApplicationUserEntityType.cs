﻿// <auto-generated />
using System;
using System.Reflection;
using Banshee5.IdentityProvider.App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable disable

namespace Banshee5.IdentityProvider.App.Data.CompiledModels
{
    internal partial class ApplicationUserEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "Banshee5.IdentityProvider.App.Models.ApplicationUser",
                typeof(ApplicationUser),
                baseEntityType);

            var id = runtimeEntityType.AddProperty(
                "Id",
                typeof(string),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                afterSaveBehavior: PropertySaveBehavior.Throw);

            var accessFailedCount = runtimeEntityType.AddProperty(
                "AccessFailedCount",
                typeof(int),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("AccessFailedCount", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<AccessFailedCount>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var concurrencyStamp = runtimeEntityType.AddProperty(
                "ConcurrencyStamp",
                typeof(string),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("ConcurrencyStamp", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<ConcurrencyStamp>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true,
                concurrencyToken: true);

            var email = runtimeEntityType.AddProperty(
                "Email",
                typeof(string),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("Email", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<Email>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true,
                maxLength: 256);

            var emailConfirmed = runtimeEntityType.AddProperty(
                "EmailConfirmed",
                typeof(bool),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("EmailConfirmed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<EmailConfirmed>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var lockoutEnabled = runtimeEntityType.AddProperty(
                "LockoutEnabled",
                typeof(bool),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("LockoutEnabled", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<LockoutEnabled>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var lockoutEnd = runtimeEntityType.AddProperty(
                "LockoutEnd",
                typeof(DateTimeOffset?),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("LockoutEnd", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<LockoutEnd>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var normalizedEmail = runtimeEntityType.AddProperty(
                "NormalizedEmail",
                typeof(string),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("NormalizedEmail", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<NormalizedEmail>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true,
                maxLength: 256);

            var normalizedUserName = runtimeEntityType.AddProperty(
                "NormalizedUserName",
                typeof(string),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("NormalizedUserName", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<NormalizedUserName>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true,
                maxLength: 256);

            var passwordHash = runtimeEntityType.AddProperty(
                "PasswordHash",
                typeof(string),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("PasswordHash", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<PasswordHash>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var phoneNumber = runtimeEntityType.AddProperty(
                "PhoneNumber",
                typeof(string),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("PhoneNumber", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<PhoneNumber>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var phoneNumberConfirmed = runtimeEntityType.AddProperty(
                "PhoneNumberConfirmed",
                typeof(bool),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("PhoneNumberConfirmed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<PhoneNumberConfirmed>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var securityStamp = runtimeEntityType.AddProperty(
                "SecurityStamp",
                typeof(string),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("SecurityStamp", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<SecurityStamp>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var twoFactorEnabled = runtimeEntityType.AddProperty(
                "TwoFactorEnabled",
                typeof(bool),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("TwoFactorEnabled", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<TwoFactorEnabled>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var userName = runtimeEntityType.AddProperty(
                "UserName",
                typeof(string),
                propertyInfo: typeof(IdentityUser<string>).GetProperty("UserName", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(IdentityUser<string>).GetField("<UserName>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true,
                maxLength: 256);

            var key = runtimeEntityType.AddKey(
                new[] { id });
            runtimeEntityType.SetPrimaryKey(key);

            var index = runtimeEntityType.AddIndex(
                new[] { normalizedEmail });
            index.AddAnnotation("Relational:Name", "EmailIndex");

            var index0 = runtimeEntityType.AddIndex(
                new[] { normalizedUserName },
                unique: true);
            index0.AddAnnotation("Relational:Name", "UserNameIndex");

            return runtimeEntityType;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "AspNetUsers");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}