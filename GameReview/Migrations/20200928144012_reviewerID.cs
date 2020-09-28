using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GameReview.Migrations
{
    public partial class reviewerID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Reviewer_ReviewerID",
                table: "Review");

            migrationBuilder.DropTable(
                name: "Reviewer");

            migrationBuilder.DropIndex(
                name: "IX_Review_ReviewerID",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Review");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewerID",
                table: "Review",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ReviewerID",
                table: "Review",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Review",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Reviewer",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviewer", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Review_ReviewerID",
                table: "Review",
                column: "ReviewerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Reviewer_ReviewerID",
                table: "Review",
                column: "ReviewerID",
                principalTable: "Reviewer",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
