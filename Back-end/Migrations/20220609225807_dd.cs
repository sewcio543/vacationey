using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class dd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Admin_AdminId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_City_ArrivalCityId",
                table: "Offer");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropIndex(
                name: "IX_Offer_AdminId",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_ArrivalCityId",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "ArrivalCityId",
                table: "Offer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Offer",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArrivalCityId",
                table: "Offer",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.AdminId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offer_AdminId",
                table: "Offer",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_ArrivalCityId",
                table: "Offer",
                column: "ArrivalCityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Admin_AdminId",
                table: "Offer",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_City_ArrivalCityId",
                table: "Offer",
                column: "ArrivalCityId",
                principalTable: "City",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
