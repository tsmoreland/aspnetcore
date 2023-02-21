﻿// <auto-generated />
using System;
using GloboTicket.Shop.Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GloboTicket.Shop.Catalog.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(EventCatalogDbContext))]
    [Migration("20230211203759_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GloboTicket.Shop.Catalog.Domain.Models.Concert", b =>
                {
                    b.Property<Guid>("ConcertId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Artist")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.HasKey("ConcertId");

                    b.ToTable("Concerts");
                });

            modelBuilder.Entity("GloboTicket.Shop.Catalog.Domain.Models.Concert", b =>
                {
                    b.OwnsOne("GloboTicket.Shop.Shared.Models.AuditDetails", "Audit", b1 =>
                        {
                            b1.Property<Guid>("ConcertId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("CreatedBy")
                                .HasMaxLength(200)
                                .HasColumnType("nvarchar(200)");

                            b1.Property<DateTime>("CreatedDate")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("datetime2")
                                .HasDefaultValue(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

                            b1.Property<string>("LastModifiedBy")
                                .HasMaxLength(200)
                                .HasColumnType("nvarchar(200)");

                            b1.Property<DateTime>("LastModifiedDate")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("datetime2")
                                .HasDefaultValue(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

                            b1.HasKey("ConcertId");

                            b1.ToTable("Concerts");

                            b1.WithOwner()
                                .HasForeignKey("ConcertId");
                        });

                    b.Navigation("Audit");
                });
#pragma warning restore 612, 618
        }
    }
}