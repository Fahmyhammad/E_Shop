using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myshop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class edit_OrderHandler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "orderHeaders",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_orderHeaders_ProductId",
                table: "orderHeaders",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderHeaders_Products_ProductId",
                table: "orderHeaders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderHeaders_Products_ProductId",
                table: "orderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_orderHeaders_ProductId",
                table: "orderHeaders");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "orderHeaders");
        }
    }
}
