using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MF.Membership.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedThumnailUrlToFilmEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Films",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Films");
        }
    }
}
