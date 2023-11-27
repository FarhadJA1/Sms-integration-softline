using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftlineIntegrationSystem.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenueId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VenueName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNotified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenueId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    More = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    NotifiedPersonPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IIKOApikey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HookPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizationId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apikey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Venues_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "IsAdmin", "LastName", "PasswordHash", "PasswordSalt" },
                values: new object[] { 1, "ramin@example.com", "Ramin", true, "Guliyev", new byte[] { 243, 144, 186, 154, 1, 123, 57, 177, 113, 216, 215, 162, 146, 20, 170, 237, 212, 110, 132, 81, 96, 142, 104, 249, 189, 232, 80, 166, 244, 124, 60, 250, 75, 90, 234, 186, 91, 110, 182, 10, 210, 41, 175, 37, 71, 95, 129, 127, 236, 85, 149, 103, 227, 242, 58, 164, 69, 12, 196, 31, 116, 245, 229, 35 }, new byte[] { 57, 53, 115, 220, 103, 224, 84, 68, 31, 238, 238, 5, 184, 49, 69, 169, 110, 80, 121, 128, 169, 189, 203, 238, 127, 24, 220, 104, 81, 43, 84, 169, 208, 163, 149, 9, 207, 123, 10, 26, 195, 109, 109, 67, 183, 204, 37, 146, 21, 5, 112, 121, 85, 5, 181, 51, 227, 187, 25, 116, 23, 33, 169, 21, 210, 235, 92, 203, 24, 129, 122, 155, 77, 216, 162, 63, 51, 173, 73, 68, 138, 95, 30, 182, 45, 71, 157, 1, 66, 211, 67, 220, 123, 127, 129, 166, 227, 240, 220, 116, 199, 55, 116, 21, 203, 156, 255, 141, 204, 29, 143, 133, 146, 210, 34, 92, 218, 93, 205, 100, 109, 58, 150, 6, 82, 187, 175, 7 } });

            migrationBuilder.CreateIndex(
                name: "IX_Venues_RestaurantId",
                table: "Venues",
                column: "RestaurantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Venues");

            migrationBuilder.DropTable(
                name: "Restaurants");
        }
    }
}
