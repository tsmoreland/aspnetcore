﻿// <auto-generated />
using System;
using GloboTicket.Shop.Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GloboTicket.Shop.Catalog.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(EventCatalogDbContext))]
    partial class EventCatalogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
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

                    b.HasData(
                        new
                        {
                            ConcertId = new Guid("ee272f8b-6096-4cb6-8625-bb4bb2d89e8b"),
                            Artist = "John Egbert",
                            ConcurrencyToken = "bb94d43c5d2540d3ab933df90565cc86",
                            Date = new DateTime(2023, 8, 12, 18, 47, 35, 205, DateTimeKind.Local).AddTicks(5821),
                            Description = "Join John for his farwell tour across 15 continents. John really needs no introduction since he has already mesmerized the world with his banjo.",
                            ImageUrl = "/img/banjo.jpg",
                            Name = "John Egbert Live",
                            Price = 0
                        },
                        new
                        {
                            ConcertId = new Guid("3448d5a4-0f72-4dd7-bf15-c14a46b26c00"),
                            Artist = "Michael Johnson",
                            ConcurrencyToken = "475ed4ab8df24bf493916ce9eb1e82ed",
                            Date = new DateTime(2023, 11, 12, 18, 47, 35, 205, DateTimeKind.Local).AddTicks(5912),
                            Description = "Michael Johnson doesn't need an introduction. His 25 concert across the globe last year were seen by thousands. Can we add you to the list?",
                            ImageUrl = "/img/michael.jpg",
                            Name = "The State of Affairs: Michael Live!",
                            Price = 0
                        },
                        new
                        {
                            ConcertId = new Guid("b419a7ca-3321-4f38-be8e-4d7b6a529319"),
                            Artist = "DJ 'The Mike'",
                            ConcurrencyToken = "033a61e9e35b4121b5296acc4eab5d5e",
                            Date = new DateTime(2023, 6, 12, 18, 47, 35, 205, DateTimeKind.Local).AddTicks(5922),
                            Description = "DJs from all over the world will compete in this epic battle for eternal fame.",
                            ImageUrl = "/img/dj.jpg",
                            Name = "Clash of the DJs",
                            Price = 0
                        },
                        new
                        {
                            ConcertId = new Guid("62787623-4c52-43fe-b0c9-b7044fb5929b"),
                            Artist = "Manuel Santinonisi",
                            ConcurrencyToken = "9deb7ce8579e4e57a7b3240111a3ad02",
                            Date = new DateTime(2023, 6, 12, 18, 47, 35, 205, DateTimeKind.Local).AddTicks(5937),
                            Description = "Get on the hype of Spanish Guitar concerts with Manuel.",
                            ImageUrl = "/img/guitar.jpg",
                            Name = "Spanish guitar hits with Manuel",
                            Price = 0
                        },
                        new
                        {
                            ConcertId = new Guid("adc42c09-08c1-4d2c-9f96-2d15bb1af299"),
                            Artist = "Nick Sailor",
                            ConcurrencyToken = "6992b812e2444f25ad3945e953a2f731",
                            Date = new DateTime(2023, 10, 12, 18, 47, 35, 205, DateTimeKind.Local).AddTicks(5945),
                            Description = "The critics are over the moon and so will you after you've watched this sing and dance extravaganza written by Nick Sailor, the man from 'My dad and sister'.",
                            ImageUrl = "/img/musical.jpg",
                            Name = "To the Moon and Back",
                            Price = 0
                        });
                });

            modelBuilder.Entity("GloboTicket.Shop.Catalog.Domain.Models.Concert", b =>
                {
                    b.OwnsOne("GloboTicket.Shop.Shared.Models.Persistence.AuditDetails", "Audit", b1 =>
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
