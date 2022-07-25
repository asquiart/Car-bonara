using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace CarbonaraWebAPI.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AddedBilltoBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Booking_RelatedBookingId",
                table: "Bill");

            migrationBuilder.DropIndex(
                name: "IX_Bill_RelatedBookingId",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "RelatedBookingId",
                table: "Bill");

            migrationBuilder.AddColumn<int>(
                name: "BillId",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_BillId",
                table: "Booking",
                column: "BillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Bill_BillId",
                table: "Booking",
                column: "BillId",
                principalTable: "Bill",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Bill_BillId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_BillId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "BillId",
                table: "Booking");

            migrationBuilder.AddColumn<int>(
                name: "RelatedBookingId",
                table: "Bill",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bill_RelatedBookingId",
                table: "Bill",
                column: "RelatedBookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Booking_RelatedBookingId",
                table: "Bill",
                column: "RelatedBookingId",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
