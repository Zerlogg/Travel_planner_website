using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelingTrips.Migrations
{
    /// <inheritdoc />
    public partial class TravelDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TravelId",
                table: "TravelDays",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TravelDayId",
                table: "TourismObjects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TravelId",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TravelId",
                table: "Accommodations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TravelDays_TravelId",
                table: "TravelDays",
                column: "TravelId");

            migrationBuilder.CreateIndex(
                name: "IX_TourismObjects_TravelDayId",
                table: "TourismObjects",
                column: "TravelDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_TravelId",
                table: "Restaurants",
                column: "TravelId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_TravelId",
                table: "Accommodations",
                column: "TravelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Travels_TravelId",
                table: "Accommodations",
                column: "TravelId",
                principalTable: "Travels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Travels_TravelId",
                table: "Restaurants",
                column: "TravelId",
                principalTable: "Travels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TourismObjects_TravelDays_TravelDayId",
                table: "TourismObjects",
                column: "TravelDayId",
                principalTable: "TravelDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelDays_Travels_TravelId",
                table: "TravelDays",
                column: "TravelId",
                principalTable: "Travels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Travels_TravelId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Travels_TravelId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_TourismObjects_TravelDays_TravelDayId",
                table: "TourismObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelDays_Travels_TravelId",
                table: "TravelDays");

            migrationBuilder.DropIndex(
                name: "IX_TravelDays_TravelId",
                table: "TravelDays");

            migrationBuilder.DropIndex(
                name: "IX_TourismObjects_TravelDayId",
                table: "TourismObjects");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_TravelId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Accommodations_TravelId",
                table: "Accommodations");

            migrationBuilder.AlterColumn<string>(
                name: "TravelId",
                table: "TravelDays",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TravelDayId",
                table: "TourismObjects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TravelId",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TravelId",
                table: "Accommodations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
