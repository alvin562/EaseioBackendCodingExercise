using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EaseioBackendCodingExercise.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GUIDRecords",
                columns: table => new
                {
                    Guid = table.Column<string>(type: "TEXT", nullable: false),
                    Expires = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    User = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GUIDRecords", x => x.Guid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GUIDRecords");
        }
    }
}
