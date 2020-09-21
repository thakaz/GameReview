using Microsoft.EntityFrameworkCore.Migrations;

namespace GameReview.Data.Migrations
{
    public partial class ChangeProsConsListToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsPoint");

            migrationBuilder.DropTable(
                name: "ProsPoint");

            migrationBuilder.AddColumn<string>(
                name: "ConsPoints",
                table: "Review",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProsPoints",
                table: "Review",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsPoints",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "ProsPoints",
                table: "Review");

            migrationBuilder.CreateTable(
                name: "ConsPoint",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewID = table.Column<int>(type: "int", nullable: true)
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
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pros = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewID = table.Column<int>(type: "int", nullable: true)
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
    }
}
