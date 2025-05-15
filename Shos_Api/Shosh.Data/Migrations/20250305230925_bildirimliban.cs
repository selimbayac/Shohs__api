using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shosh.Data.Migrations
{
    /// <inheritdoc />
    public partial class bildirimliban : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlogId",
                table: "Complaints",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Complaints",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EntryId",
                table: "Complaints",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Complaints",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BlogId",
                table: "Comments",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Blogs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_BlogId",
                table: "Complaints",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_CommentId",
                table: "Complaints",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_EntryId",
                table: "Complaints",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogId",
                table: "Comments",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Blogs_BlogId",
                table: "Comments",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Blogs_BlogId",
                table: "Complaints",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Comments_CommentId",
                table: "Complaints",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Entries_EntryId",
                table: "Complaints",
                column: "EntryId",
                principalTable: "Entries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Blogs_BlogId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Blogs_BlogId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Comments_CommentId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Entries_EntryId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_BlogId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_CommentId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_EntryId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Comments_BlogId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "EntryId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Blogs",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
