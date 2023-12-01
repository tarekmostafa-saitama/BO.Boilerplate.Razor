using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSSQL.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class islockedCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLockedDown",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLockedDown",
                table: "AspNetUsers");
        }
    }
}
