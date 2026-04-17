using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceiroBackend.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableCashFlowChangeColumnNameToMonth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mouth",
                table: "CashFlows",
                newName: "Month");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Month",
                table: "CashFlows",
                newName: "Mouth");
        }
    }
}
