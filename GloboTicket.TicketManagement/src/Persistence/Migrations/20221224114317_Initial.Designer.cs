﻿// <auto-generated />
using System;
using GloboTicket.TicketManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GloboTicket.TicketManagement.Persistence.Migrations
{
    [DbContext(typeof(GloboTicketDbContext))]
    [Migration("20221224114317_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.1");

            modelBuilder.Entity("GloboTicket.TicketManagement.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = new Guid("2ab3d254-c85a-4232-a061-d0c3f5516d2e"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Concerts"
                        },
                        new
                        {
                            CategoryId = new Guid("5b94f780-cf1f-484f-9403-4a3531b26c1f"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Musicals"
                        },
                        new
                        {
                            CategoryId = new Guid("96c73b1d-cc44-4ac4-b034-ef503ba298ba"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Plays"
                        },
                        new
                        {
                            CategoryId = new Guid("50ce56e9-8c15-4ce1-8f30-501b25394e55"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Conferences"
                        });
                });

            modelBuilder.Entity("GloboTicket.TicketManagement.Domain.Entities.Event", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Artist")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("Price")
                        .HasColumnType("INTEGER");

                    b.HasKey("EventId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Events");

                    b.HasData(
                        new
                        {
                            EventId = new Guid("2f620341-fa98-48b1-8af4-5418b1e109e1"),
                            Artist = "John Egbert",
                            CategoryId = new Guid("2ab3d254-c85a-4232-a061-d0c3f5516d2e"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Date = new DateTime(2023, 6, 24, 11, 43, 17, 587, DateTimeKind.Local).AddTicks(1827),
                            Description = "Join John for his farewell tour across 15 continents",
                            ImageUrl = "https://example.com/images/example.jpg",
                            Name = "John Egbert Live",
                            Price = 65
                        },
                        new
                        {
                            EventId = new Guid("b5775cca-5533-4be5-88f8-f0edc7be6e04"),
                            Artist = "Michael Johnson",
                            CategoryId = new Guid("2ab3d254-c85a-4232-a061-d0c3f5516d2e"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Date = new DateTime(2023, 9, 24, 11, 43, 17, 587, DateTimeKind.Local).AddTicks(1905),
                            Description = "Michael Johnson doesn't need an introduction",
                            ImageUrl = "https://example.com/images/example.jpg",
                            Name = "The State of Affairs: Michael Live!",
                            Price = 85
                        },
                        new
                        {
                            EventId = new Guid("9f6ba16e-a130-4ae1-9f0c-07f695636b4b"),
                            Artist = "DJ 'The Mike'",
                            CategoryId = new Guid("2ab3d254-c85a-4232-a061-d0c3f5516d2e"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Date = new DateTime(2023, 4, 24, 11, 43, 17, 587, DateTimeKind.Local).AddTicks(1915),
                            Description = "DJs from all over the world will complete in this empic battle for eternal fame.",
                            ImageUrl = "https://example.com/images/example.jpg",
                            Name = "Clash of the DJs",
                            Price = 85
                        });
                });

            modelBuilder.Entity("GloboTicket.TicketManagement.Domain.Entities.Order", b =>
                {
                    b.Property<Guid>("EventId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("OrderPaid")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("OrderPlaced")
                        .HasColumnType("TEXT");

                    b.Property<int>("OrderTotal")
                        .HasColumnType("INTEGER");

                    b.HasKey("EventId", "UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("GloboTicket.TicketManagement.Domain.Entities.Event", b =>
                {
                    b.HasOne("GloboTicket.TicketManagement.Domain.Entities.Category", "Category")
                        .WithMany("Events")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("GloboTicket.TicketManagement.Domain.Entities.Category", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
