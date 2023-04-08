using Microsoft.EntityFrameworkCore.Migrations;

namespace TopLearn.DataLayer.Migrations
{
    public partial class Mig_IsDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPay",
                table: "Wallets",
                newName: "IsPay");

            migrationBuilder.RenameColumn(
                name: "WalletId",
                table: "Wallets",
                newName: "WalletId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsPay",
                table: "Wallets",
                newName: "IsPay");

            migrationBuilder.RenameColumn(
                name: "WalletId",
                table: "Wallets",
                newName: "WalletId");
        }
    }
}
