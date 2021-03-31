using Microsoft.EntityFrameworkCore.Migrations;

namespace IdpServer.Persistence.Migrations.ConfigurationDb
{
    public partial class Update5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequireResourceIndicator",
                table: "ApiResources",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequireResourceIndicator",
                table: "ApiResources");
        }
    }
}
