using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentValidationAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UploadDocumentLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActedAt",
                table: "ValidationSteps",
                newName: "ApprovedAt");

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "Documents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "ApprovedAt",
                table: "ValidationSteps",
                newName: "ActedAt");
        }
    }
}
