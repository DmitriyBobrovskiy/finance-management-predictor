using Microsoft.EntityFrameworkCore.Migrations;

namespace finance_management_backend.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_LoanTypes_LoanTypeId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "LoanTypeId",
                table: "Transactions",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_LoanTypes_LoanTypeId",
                table: "Transactions",
                column: "LoanTypeId",
                principalTable: "LoanTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_LoanTypes_LoanTypeId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "LoanTypeId",
                table: "Transactions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_LoanTypes_LoanTypeId",
                table: "Transactions",
                column: "LoanTypeId",
                principalTable: "LoanTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
