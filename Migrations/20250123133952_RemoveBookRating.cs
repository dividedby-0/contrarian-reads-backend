using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace contrarian_reads_backend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBookRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "Books",
                type: "real",
                nullable: true);
        }
    }
}
