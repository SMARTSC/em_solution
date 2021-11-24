using Microsoft.EntityFrameworkCore.Migrations;

namespace EM.Print.Cloud.Migrations
{
    public partial class Extend3PrintOrderSchemaMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "targetcontenteventimageid",
                table: "printorders",
                newName: "targetcontenteventlogoimageid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "targetcontenteventlogoimageid",
                table: "printorders",
                newName: "targetcontenteventimageid");
        }
    }
}
