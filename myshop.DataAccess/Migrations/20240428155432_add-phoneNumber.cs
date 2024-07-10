using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myshop.myshop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addphoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "orderHeaders",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "orderHeaders",
                newName: "Number");
        }
    }
}
