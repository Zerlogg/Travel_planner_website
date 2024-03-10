using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelingTrips.Migrations
{
    /// <inheritdoc />
    public partial class Travel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Travels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Budget = table.Column<double>(type: "float", nullable: true),
                    Accommodation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Preferences = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Travels", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Travels",
                columns: new[] { "Id", "Accommodation", "Budget", "City", "Description", "EndDate", "Preferences", "StartDate" },
                values: new object[,]
                {
                    { 1, "Hotel", 412.39999999999998, "Paris", "I love Paris", new DateTime(2024, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "I wanna see Eiffel tower", new DateTime(2024, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Camping", 500.19999999999999, "Washington", "I love Washington", new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "I wanna see the White House", new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Travels");
        }
    }
}
