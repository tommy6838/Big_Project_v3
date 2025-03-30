using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Big_Project_v3.Migrations
{
    /// <inheritdoc />
    public partial class AddIsReviewLockedToReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IsReviewLocked",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReviewLocked",
                table: "Reviews");
        }
    }
}
