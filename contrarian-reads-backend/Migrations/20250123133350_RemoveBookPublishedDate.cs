using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace contrarian_reads_backend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBookPublishedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedDate",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedDate",
                table: "Books",
                type: "datetime2",
                nullable: true);
        }
    }
}
