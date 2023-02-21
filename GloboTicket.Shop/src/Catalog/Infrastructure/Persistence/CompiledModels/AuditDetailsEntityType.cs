﻿// <auto-generated />
using System;
using System.Reflection;
using GloboTicket.Shop.Catalog.Domain.Models;
using GloboTicket.Shop.Shared.Models.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable enable

namespace GloboTicket.Shop.Catalog.Infrastructure.Persistence.CompiledModels
{
    internal partial class AuditDetailsEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType? baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "GloboTicket.Shop.Shared.Models.AuditDetails",
                typeof(AuditDetails),
                baseEntityType);

            var concertId = runtimeEntityType.AddProperty(
                "ConcertId",
                typeof(Guid),
                afterSaveBehavior: PropertySaveBehavior.Throw);
            concertId.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            var createdBy = runtimeEntityType.AddProperty(
                "CreatedBy",
                typeof(string),
                propertyInfo: typeof(AuditDetails).GetProperty("CreatedBy", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(AuditDetails).GetField("<CreatedBy>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true,
                maxLength: 200);
            createdBy.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            var createdDate = runtimeEntityType.AddProperty(
                "CreatedDate",
                typeof(DateTime),
                propertyInfo: typeof(AuditDetails).GetProperty("CreatedDate", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(AuditDetails).GetField("<CreatedDate>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd);
            createdDate.AddAnnotation("Relational:DefaultValue", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            createdDate.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            var lastModifiedBy = runtimeEntityType.AddProperty(
                "LastModifiedBy",
                typeof(string),
                propertyInfo: typeof(AuditDetails).GetProperty("LastModifiedBy", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(AuditDetails).GetField("<LastModifiedBy>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true,
                maxLength: 200);
            lastModifiedBy.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            var lastModifiedDate = runtimeEntityType.AddProperty(
                "LastModifiedDate",
                typeof(DateTime),
                propertyInfo: typeof(AuditDetails).GetProperty("LastModifiedDate", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(AuditDetails).GetField("<LastModifiedDate>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd);
            lastModifiedDate.AddAnnotation("Relational:DefaultValue", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            lastModifiedDate.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            var key = runtimeEntityType.AddKey(
                new[] { concertId });
            runtimeEntityType.SetPrimaryKey(key);

            return runtimeEntityType;
        }

        public static RuntimeForeignKey CreateForeignKey1(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
        {
            var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("ConcertId")! },
                principalEntityType.FindKey(new[] { principalEntityType.FindProperty("ConcertId")! })!,
                principalEntityType,
                deleteBehavior: DeleteBehavior.Cascade,
                unique: true,
                required: true,
                ownership: true);

            var audit = principalEntityType.AddNavigation("Audit",
                runtimeForeignKey,
                onDependent: false,
                typeof(AuditDetails),
                propertyInfo: typeof(Concert).GetProperty("Audit", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Concert).GetField("<Audit>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                eagerLoaded: true);

            return runtimeForeignKey;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "Concerts");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}