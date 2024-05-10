using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelingTrips.Migrations
{
    /// <inheritdoc />
    public partial class TravelImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Travels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Travels",
                keyColumn: "Id",
                keyValue: 1,
                column: "Image",
                value: null);

            migrationBuilder.UpdateData(
                table: "Travels",
                keyColumn: "Id",
                keyValue: 2,
                column: "Image",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Travels");
        }
    }
}
