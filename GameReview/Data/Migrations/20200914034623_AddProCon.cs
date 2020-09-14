using Microsoft.EntityFrameworkCore.Migrations;

namespace GameReview.Data.Migrations
{
    public partial class AddProCon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsPoint",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cons = table.Column<string>(nullable: true),
                    ReviewID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsPoint", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ConsPoint_Review_ReviewID",
                        column: x => x.ReviewID,
                        principalTable: "Review",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProsPoint",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pros = table.Column<string>(nullable: true),
                    ReviewID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProsPoint", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProsPoint_Review_ReviewID",
                        column: x => x.ReviewID,
                        principalTable: "Review",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsPoint_ReviewID",
                table: "ConsPoint",
                column: "ReviewID");

            migrationBuilder.CreateIndex(
                name: "IX_ProsPoint_ReviewID",
                table: "ProsPoint",
                column: "ReviewID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsPoint");

            migrationBuilder.DropTable(
                name: "ProsPoint");
        }
    }
}
