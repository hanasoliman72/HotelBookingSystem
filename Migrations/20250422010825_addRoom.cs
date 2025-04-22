using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class addRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "rooms",
                columns: new[] { "ID", "Floor", "ImageUrl", "Price", "Rate", "RoomClassID", "Status", "View" },
                values: new object[] { 6, 5, "/assets/img/rooms/room6.jpg", 250.00m, 5, 2, 0, "Mountain View" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "rooms",
                keyColumn: "ID",
                keyValue: 6);
        }
    }
}
