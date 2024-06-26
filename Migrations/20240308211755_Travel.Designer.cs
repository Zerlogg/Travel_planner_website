﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TravelingTrips.Data;

#nullable disable

namespace TravelingTrips.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240308211755_Travel")]
    partial class Travel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TravelingTrips.Models.Folder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Folders");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Title = "France"
                        },
                        new
                        {
                            Id = 2,
                            Title = "America"
                        });
                });

            modelBuilder.Entity("TravelingTrips.Models.Travel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Accommodation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Budget")
                        .HasColumnType("float");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Preferences")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Travels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Accommodation = "Hotel",
                            Budget = 412.39999999999998,
                            City = "Paris",
                            Description = "I love Paris",
                            EndDate = new DateTime(2024, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Preferences = "I wanna see Eiffel tower",
                            StartDate = new DateTime(2024, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            Accommodation = "Camping",
                            Budget = 500.19999999999999,
                            City = "Washington",
                            Description = "I love Washington",
                            EndDate = new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Preferences = "I wanna see the White House",
                            StartDate = new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("TravelingTrips.Models.VideoGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Publisher")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ReleaseYear")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VideoGames");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Publisher = "Emils",
                            ReleaseYear = 2005,
                            Title = "Osas"
                        },
                        new
                        {
                            Id = 2,
                            Publisher = "Alex",
                            ReleaseYear = 2001,
                            Title = "Basas"
                        },
                        new
                        {
                            Id = 3,
                            Publisher = "Ilja",
                            ReleaseYear = 2002,
                            Title = "Kasas"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
