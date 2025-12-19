using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceiroBackend.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableContasAddUserIdCollumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Contas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Contas_UserId",
                table: "Contas",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contas_Users_UserId",
                table: "Contas",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contas_Users_UserId",
                table: "Contas");

            migrationBuilder.DropIndex(
                name: "IX_Contas_UserId",
                table: "Contas");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Contas");
        }
    }
}
