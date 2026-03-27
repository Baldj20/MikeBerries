using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserKeyNameToIdentityId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Auth0Id",
                table: "Users",
                newName: "IdentityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdentityId",
                table: "Users",
                newName: "Auth0Id");
        }
    }
}
