using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTest_KawanLama.Migrations
{
    public partial class ToDoUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Todo",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Todo");
        }
    }
}
