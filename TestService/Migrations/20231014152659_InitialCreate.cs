using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    BasicClassId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TestField = table.Column<string>(type: "TEXT", nullable: true),
                    Refnr = table.Column<int>(type: "INTEGER", nullable: false),
                    Oprettet = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.BasicClassId);
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    BasicEntryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BasicClassId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ValueToLoad = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.BasicEntryId);
                    table.ForeignKey(
                        name: "FK_Entries_Classes_BasicClassId",
                        column: x => x.BasicClassId,
                        principalTable: "Classes",
                        principalColumn: "BasicClassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BasicClassId",
                table: "Entries",
                column: "BasicClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "Classes");
        }
    }
}
