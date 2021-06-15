using Microsoft.EntityFrameworkCore.Migrations;

namespace AirVinyl.Migrations
{
    public partial class AddDefaultValueForGender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "People",
                nullable: false,
                defaultValue: 3,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "People",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 3);
        }
    }
}
