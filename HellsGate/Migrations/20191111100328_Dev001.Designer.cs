﻿// <auto-generated />
using System;
using HellsGate.Models;
using HellsGate.Models.Context;
using HellsGate.Models.DatabaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HellsGate.Migrations
{
    [DbContext(typeof(HellsGateContext))]
    [Migration("20191111100328_Dev001")]
    partial class Dev001
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HellsGate.Models.AccessModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AccessTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("GrantedAccess")
                        .HasColumnType("bit");

                    b.Property<string>("PeopleEntered")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Plate")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Access");
                });

            modelBuilder.Entity("HellsGate.Models.CarAnagraphicModel", b =>
                {
                    b.Property<string>("LicencePlate")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Colour")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModify")
                        .HasColumnType("datetime2");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LicencePlate");

                    b.HasIndex("OwnerId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("HellsGate.Models.CardModel", b =>
                {
                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("CardNumber");

                    b.ToTable("CardModels");
                });

            modelBuilder.Entity("HellsGate.Models.MainMenuModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AuthLevel")
                        .HasColumnType("int");

                    b.Property<string>("Controller")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MainMenu");
                });

            modelBuilder.Entity("HellsGate.Models.PeopleAnagraphicModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("CardNumber1")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModify")
                        .HasColumnType("datetime2");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CardNumber1");

                    b.ToTable("Peoples");

                    b.HasData(
                        new
                        {
                            Id = "0885da9b-ba4f-4698-9419-ff8eb8f9d3ec",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "1a8baee8-a724-4529-b8d2-8f200e3418a4",
                            EmailConfirmed = false,
                            LastModify = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LockoutEnabled = false,
                            Password = "ZmbpVrIuW/wiGze/tuyOaUCrA+onxN5OaHtuKANmccGLvETB",
                            PhoneNumberConfirmed = false,
                            TwoFactorEnabled = false,
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("HellsGate.Models.CarAnagraphicModel", b =>
                {
                    b.HasOne("HellsGate.Models.PeopleAnagraphicModel", "Owner")
                        .WithMany("Cars")
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("HellsGate.Models.PeopleAnagraphicModel", b =>
                {
                    b.HasOne("HellsGate.Models.CardModel", "CardNumber")
                        .WithMany()
                        .HasForeignKey("CardNumber1");

                    b.OwnsOne("HellsGate.Models.AutorizationLevelModel", "AutorizationLevel", b1 =>
                        {
                            b1.Property<string>("PeopleAnagraphicModelId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("AuthName")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("AuthValue")
                                .HasColumnType("int");

                            b1.Property<DateTime>("CreationDate")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime>("ExpirationDate")
                                .HasColumnType("datetime2");

                            b1.Property<int>("Id")
                                .HasColumnName("Id")
                                .HasColumnType("int");

                            b1.HasKey("PeopleAnagraphicModelId");

                            b1.ToTable("Autorizations");

                            b1.WithOwner()
                                .HasForeignKey("PeopleAnagraphicModelId");
                        });

                    b.OwnsOne("HellsGate.Models.SafeAuthModel", "SafeAuthModel", b1 =>
                        {
                            b1.Property<string>("PeopleAnagraphicModelId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<int>("AutId")
                                .HasColumnType("int");

                            b1.Property<string>("Control")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<DateTime>("DtIns")
                                .HasColumnType("datetime2");

                            b1.Property<int>("Id")
                                .HasColumnName("Id")
                                .HasColumnType("int");

                            b1.Property<string>("UserId")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("PeopleAnagraphicModelId");

                            b1.ToTable("SafeAuthModels");

                            b1.WithOwner()
                                .HasForeignKey("PeopleAnagraphicModelId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
