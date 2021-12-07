using Microsoft.EntityFrameworkCore.Migrations;

namespace TrabajoPractico4.Migrations
{
    public partial class sexta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_categoryid",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Purchases",
                newName: "purchaseId");

            migrationBuilder.RenameColumn(
                name: "categoryid",
                table: "Products",
                newName: "categoryId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Products",
                newName: "productId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_categoryid",
                table: "Products",
                newName: "IX_Products_categoryId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Categories",
                newName: "categoryId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Cart",
                newName: "cartId");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_categoryId",
                table: "Products",
                column: "categoryId",
                principalTable: "Categories",
                principalColumn: "categoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_categoryId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "purchaseId",
                table: "Purchases",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "categoryId",
                table: "Products",
                newName: "categoryid");

            migrationBuilder.RenameColumn(
                name: "productId",
                table: "Products",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Products_categoryId",
                table: "Products",
                newName: "IX_Products_categoryid");

            migrationBuilder.RenameColumn(
                name: "categoryId",
                table: "Categories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "cartId",
                table: "Cart",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Products",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Products",
                type: "varchar(250)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Categories",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_categoryid",
                table: "Products",
                column: "categoryid",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
