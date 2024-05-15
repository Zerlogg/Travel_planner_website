using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelingTrips.Migrations
{
    /// <inheritdoc />
    public partial class TravelChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Travels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Travels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.RenameColumn(
                name: "Accommodation",
                table: "Travels",
                newName: "ChatHistory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChatHistory",
                table: "Travels",
                newName: "Accommodation");

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "Color", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, null, "France", null },
                    { 2, null, "America", null }
                });

            migrationBuilder.InsertData(
                table: "Travels",
                columns: new[] { "Id", "Accommodation", "Budget", "City", "Description", "EndDate", "Image", "Preferences", "StartDate", "UserId" },
                values: new object[,]
                {
                    { 1, "Hotel", 412.39999999999998, "Paris", "I love Paris", new DateTime(2024, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "I wanna see Eiffel tower", new DateTime(2024, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, "Camping", 500.19999999999999, "Washington", "I love Washington", new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "I wanna see the White House", new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });
        }
    }
}
