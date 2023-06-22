﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TennisByTheSea.Infrastructure.Persistence;

#nullable disable

namespace TennisByTheSea.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(TennisByTheSeaDbContext))]
    partial class TennisByTheSeaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.7");

            modelBuilder.Entity("TennisByTheSea.Domain.Models.ConfigurationEntity", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("ConfigurationEntity");
                });

            modelBuilder.Entity("TennisByTheSea.Domain.Models.Court", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Courts");
                });

            modelBuilder.Entity("TennisByTheSea.Domain.Models.CourtBooking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("_courtId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("_memberId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("_courtId");

                    b.HasIndex("_memberId");

                    b.ToTable("CourtBookings");
                });

            modelBuilder.Entity("TennisByTheSea.Domain.Models.CourtMaintenanceSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourtId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CourtIsClosed")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("WorkTitle")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("_courtId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CourtId");

                    b.ToTable("CourtMaintenanceSchedule");
                });

            modelBuilder.Entity("TennisByTheSea.Domain.Models.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Forename")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("_forename")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("_joinDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("_surname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("TennisByTheSea.Domain.Models.CourtBooking", b =>
                {
                    b.HasOne("TennisByTheSea.Domain.Models.Court", "Court")
                        .WithMany("Bookings")
                        .HasForeignKey("_courtId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TennisByTheSea.Domain.Models.Member", "Member")
                        .WithMany("CourtBookings")
                        .HasForeignKey("_memberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Court");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("TennisByTheSea.Domain.Models.CourtMaintenanceSchedule", b =>
                {
                    b.HasOne("TennisByTheSea.Domain.Models.Court", "Court")
                        .WithMany()
                        .HasForeignKey("CourtId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Court");
                });

            modelBuilder.Entity("TennisByTheSea.Domain.Models.Court", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("TennisByTheSea.Domain.Models.Member", b =>
                {
                    b.Navigation("CourtBookings");
                });
#pragma warning restore 612, 618
        }
    }
}
