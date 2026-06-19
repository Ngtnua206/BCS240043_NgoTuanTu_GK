using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTKTGK_BCS240043_NgoTuanTu.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomTypes_BCS240043",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypes_BCS240043", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms_BCS240043",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Area = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms_BCS240043", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_BCS240043_RoomTypes_BCS240043_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes_BCS240043",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoomImages_BCS240043",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsThumbnail = table.Column<bool>(type: "bit", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomImages_BCS240043", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomImages_BCS240043_Rooms_BCS240043_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms_BCS240043",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomImages_BCS240043_RoomId",
                table: "RoomImages_BCS240043",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BCS240043_Name_RoomTypeId",
                table: "Rooms_BCS240043",
                columns: new[] { "Name", "RoomTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BCS240043_RoomTypeId",
                table: "Rooms_BCS240043",
                column: "RoomTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomImages_BCS240043");

            migrationBuilder.DropTable(
                name: "Rooms_BCS240043");

            migrationBuilder.DropTable(
                name: "RoomTypes_BCS240043");
        }
    }
}
