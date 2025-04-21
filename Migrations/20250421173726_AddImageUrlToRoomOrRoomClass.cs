using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToRoomOrRoomClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "rooms",
                keyColumn: "ID",
                keyValue: 1,
                column: "ImageUrl",
                value: "~/assets/img/rooms/room1.jpg");

            migrationBuilder.UpdateData(
                table: "rooms",
                keyColumn: "ID",
                keyValue: 2,
                column: "ImageUrl",
                value: "~/assets/img/rooms/room2.jpg");

            migrationBuilder.UpdateData(
                table: "rooms",
                keyColumn: "ID",
                keyValue: 3,
                column: "ImageUrl",
                value: "assets/img/rooms/room3.jpg");

            migrationBuilder.UpdateData(
                table: "rooms",
                keyColumn: "ID",
                keyValue: 4,
                column: "ImageUrl",
                value: "assets/img/rooms/room4.jpg");

            migrationBuilder.UpdateData(
                table: "rooms",
                keyColumn: "ID",
                keyValue: 5,
                column: "ImageUrl",
                value: "assets/img/rooms/room5.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "rooms");
        }
    }
}
