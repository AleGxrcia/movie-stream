using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStream.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameImagePathToImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "TvSeries",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Movies",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "TvSeries",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Movies",
                newName: "ImagePath");
        }
    }
}
