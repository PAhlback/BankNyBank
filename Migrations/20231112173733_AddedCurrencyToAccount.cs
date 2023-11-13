using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankNyBank.Migrations
{
    public partial class AddedCurrencyToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Accounts");
        }
    }
}
