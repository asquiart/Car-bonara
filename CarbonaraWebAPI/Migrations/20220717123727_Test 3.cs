using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace CarbonaraWebAPI.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class Test3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Station_StartstationId",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "StartstationId",
                table: "Booking",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Station_StartstationId",
                table: "Booking",
                column: "StartstationId",
                principalTable: "Station",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Station_StartstationId",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "StartstationId",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Station_StartstationId",
                table: "Booking",
                column: "StartstationId",
                principalTable: "Station",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
