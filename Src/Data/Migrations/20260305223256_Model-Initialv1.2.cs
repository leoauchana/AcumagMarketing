using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ModelInitialv12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExpirationDate",
                table: "Quotes",
                type: "varchar(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar");

            migrationBuilder.AlterColumn<string>(
                name: "Dni",
                table: "Employees",
                type: "varchar(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar");

            migrationBuilder.AlterColumn<string>(
                name: "Dni",
                table: "Customers",
                type: "varchar(9)",
                maxLength: 9,
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

            migrationBuilder.AlterColumn<string>(
                name: "Dni",
                table: "Employees",
                type: "varchar",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(9)",
                oldMaxLength: 9);

            migrationBuilder.AlterColumn<string>(
                name: "Dni",
                table: "Customers",
                type: "varchar",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(9)",
                oldMaxLength: 9);
        }
    }
}
