using Microsoft.EntityFrameworkCore.Migrations;

namespace GameReview.Data.Migrations
{
    public partial class AddReviewComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Review",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Review");
        }
    }
}
