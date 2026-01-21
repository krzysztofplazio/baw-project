using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Baw.Migrations
{
    /// <inheritdoc />
    public partial class ChangeClientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "clients");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "clients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_clients_IdentityUserId",
                table: "clients",
                column: "IdentityUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_clients_AspNetUsers_IdentityUserId",
                table: "clients",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clients_AspNetUsers_IdentityUserId",
                table: "clients");

            migrationBuilder.DropIndex(
                name: "IX_clients_IdentityUserId",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "clients");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "clients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
