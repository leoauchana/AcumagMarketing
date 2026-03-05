using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ModelInitialv11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Document");

            migrationBuilder.AlterColumn<string>(
                name: "ExpirationDate",
                table: "Quotes",
                type: "varchar(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExpirationDate",
                table: "Quotes",
                type: "varchar",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Document",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Size",
                table: "Document",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Document",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
