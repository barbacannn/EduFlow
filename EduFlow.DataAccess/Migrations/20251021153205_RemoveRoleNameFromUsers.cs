using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduFlow.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoleNameFromUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
