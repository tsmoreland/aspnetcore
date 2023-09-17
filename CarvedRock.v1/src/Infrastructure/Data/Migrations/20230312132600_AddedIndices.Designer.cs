﻿// <auto-generated />
using System;
using CarvedRock.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarvedRock.Infrastructure.Data.Migrations
{
    [DbContext(typeof(CarvedRockDbContext))]
    [Migration("20230312132600_AddedIndices")]
    partial class AddedIndices
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CarvedRock.Domain.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Checksum")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("checksum")
                        .HasComputedColumnSql("CONVERT(VARBINARY(1024),CHECKSUM([username],[first_name],[last_name]))");

                    b.Property<string>("ConsumerTypeBacking")
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("consumer_type");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("last_name");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("status");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("username");

                    b.Property<DateTime>("lastUpdatedBacking")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                        .HasColumnName("last_updated");

                    b.HasKey("Id");

                    b.HasAlternateKey("Username");

                    b.HasIndex("FirstName", "LastName");

                    b.ToTable("customers", (string)null);
                });

            modelBuilder.Entity("CarvedRock.Domain.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("description");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(22,2)")
                        .HasColumnName("price");

                    b.Property<decimal>("PriceAfterVat")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("decimal(22,2)")
                        .HasColumnName("price_after_vat")
                        .HasComputedColumnSql("[price]*1.20");

                    b.Property<double>("Weight")
                        .HasColumnType("float(36)")
                        .HasColumnName("unit_weight");

                    b.HasKey("Id");

                    b.ToTable("items", (string)null);
                });

            modelBuilder.Entity("CarvedRock.Domain.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int")
                        .HasColumnName("customer_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("Name");

                    b.ToTable("orders", (string)null);
                });

            modelBuilder.Entity("CarvedRock.Domain.ValueObjects.CustomerOrder", b =>
                {
                    b.Property<int>("CustomerId")
                        .HasColumnType("int")
                        .HasColumnName("customers_id");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("customers_name");

                    b.Property<string>("ItemDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("item_description");

                    b.Property<int>("ItemId")
                        .HasColumnType("int")
                        .HasColumnName("item_id");

                    b.Property<int>("OrderId")
                        .HasColumnType("int")
                        .HasColumnName("orders_id");

                    b.Property<string>("OrderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("orders_name");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(22,2)")
                        .HasColumnName("price");

                    b.Property<decimal>("PriceAfterVat")
                        .HasColumnType("decimal(22,2)")
                        .HasColumnName("price_after_vat");

                    b.ToTable((string)null);

                    b.ToView("customer_orders_view", (string)null);
                });

            modelBuilder.Entity("CarvedRock.Domain.ValueObjects.StatusOverview", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable((string)null);

                    b.ToView("status_overview", (string)null);
                });

            modelBuilder.Entity("ItemOrder", b =>
                {
                    b.Property<int>("ItemsId")
                        .HasColumnType("int");

                    b.Property<int>("OrdersId")
                        .HasColumnType("int");

                    b.HasKey("ItemsId", "OrdersId");

                    b.HasIndex("OrdersId");

                    b.ToTable("ItemOrder");
                });

            modelBuilder.Entity("Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int")
                        .HasColumnName("customer_id");

                    b.Property<int>("DiscountRate")
                        .HasColumnType("int")
                        .HasColumnName("discount_rate");

                    b.Property<string>("Nickname")
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("nickname");

                    b.HasKey("Id");

                    b.ToTable("tag", (string)null);
                });

            modelBuilder.Entity("CarvedRock.Domain.Entities.Customer", b =>
                {
                    b.OwnsOne("CarvedRock.Domain.ValueObjects.Address", "BillToAddress", b1 =>
                        {
                            b1.Property<int>("CustomerId")
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(50)
                                .IsUnicode(true)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("billto_city");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(50)
                                .IsUnicode(true)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("billto_country");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(50)
                                .IsUnicode(true)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("billto_street");

                            b1.HasKey("CustomerId");

                            b1.ToTable("customers");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.OwnsOne("CarvedRock.Domain.ValueObjects.Address", "ShipToAddress", b1 =>
                        {
                            b1.Property<int>("CustomerId")
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(50)
                                .IsUnicode(true)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("shipto_city");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(50)
                                .IsUnicode(true)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("shipto_country");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(50)
                                .IsUnicode(true)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("shipto_street");

                            b1.HasKey("CustomerId");

                            b1.ToTable("customers");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("BillToAddress")
                        .IsRequired();

                    b.Navigation("ShipToAddress")
                        .IsRequired();
                });

            modelBuilder.Entity("CarvedRock.Domain.Entities.Order", b =>
                {
                    b.HasOne("CarvedRock.Domain.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("ItemOrder", b =>
                {
                    b.HasOne("CarvedRock.Domain.Entities.Item", null)
                        .WithMany()
                        .HasForeignKey("ItemsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarvedRock.Domain.Entities.Order", null)
                        .WithMany()
                        .HasForeignKey("OrdersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CarvedRock.Domain.Entities.Customer", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
