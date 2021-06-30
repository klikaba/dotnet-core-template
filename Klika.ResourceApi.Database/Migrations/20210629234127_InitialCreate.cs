using Microsoft.EntityFrameworkCore.Migrations;

namespace Klika.ResourceApi.Database.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TemplateTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateProperty1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateProperty2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TemplateProperty3 = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateTable", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateTable");
        }
    }
}
