using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceiroBackend.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableUsersAddColumnEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "varchar(254)",
                nullable: false,
                defaultValue: ""
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Email", table: "Users");
        }
    }
}
