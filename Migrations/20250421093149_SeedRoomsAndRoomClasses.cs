using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoomsAndRoomClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "endUsers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_endUsers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "roomClasses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfBeds = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roomClasses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomClassID = table.Column<int>(type: "int", nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rate = table.Column<int>(type: "int", nullable: false),
                    View = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.ID);
                    table.ForeignKey(
                        name: "FK_rooms_roomClasses_RoomClassID",
                        column: x => x.RoomClassID,
                        principalTable: "roomClasses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestID = table.Column<int>(type: "int", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_bookings_endUsers_GuestID",
                        column: x => x.GuestID,
                        principalTable: "endUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bookings_rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "rooms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "feedbacks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    FeedbackDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedbacks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_feedbacks_bookings_BookingID",
                        column: x => x.BookingID,
                        principalTable: "bookings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "roomClasses",
                columns: new[] { "ID", "Description", "NumberOfBeds", "Type" },
                values: new object[,]
                {
                    { 1, "A spacious deluxe room with sea view.", 2, "Deluxe" },
                    { 2, "A cozy standard room perfect for solo travelers.", 1, "Standard" },
                    { 3, "Luxurious suite with a private living area.", 3, "Suite" }
                });

            migrationBuilder.InsertData(
                table: "rooms",
                columns: new[] { "ID", "Floor", "Price", "Rate", "RoomClassID", "Status", "View" },
                values: new object[,]
                {
                    { 1, 1, 120.00m, 5, 1, 0, "Sea View" },
                    { 2, 2, 90.00m, 3, 2, 1, "City View" },
                    { 3, 1, 150.00m, 5, 1, 0, "Sea View" },
                    { 4, 3, 75.00m, 2, 2, 2, "Garden View" },
                    { 5, 4, 200.00m, 5, 3, 0, "Mountain View" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_GuestID",
                table: "bookings",
                column: "GuestID");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_RoomID",
                table: "bookings",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_feedbacks_BookingID",
                table: "feedbacks",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_RoomClassID",
                table: "rooms",
                column: "RoomClassID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feedbacks");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "endUsers");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "roomClasses");
        }
    }
}
