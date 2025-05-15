using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shosh.Data.Migrations
{
    /// <inheritdoc />
    public partial class yorum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Entries_EntryId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Users_UserId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_EntryLike_Entries_EntryId",
                table: "EntryLike");

            migrationBuilder.DropForeignKey(
                name: "FK_EntryLike_Users_UserId",
                table: "EntryLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EntryLike",
                table: "EntryLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "EntryLike",
                newName: "EntryLikes");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_EntryLike_UserId",
                table: "EntryLikes",
                newName: "IX_EntryLikes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EntryLike_EntryId",
                table: "EntryLikes",
                newName: "IX_EntryLikes_EntryId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_UserId",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_EntryId",
                table: "Comments",
                newName: "IX_Comments_EntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntryLikes",
                table: "EntryLikes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Entries_EntryId",
                table: "Comments",
                column: "EntryId",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntryLikes_Entries_EntryId",
                table: "EntryLikes",
                column: "EntryId",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntryLikes_Users_UserId",
                table: "EntryLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Entries_EntryId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_EntryLikes_Entries_EntryId",
                table: "EntryLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_EntryLikes_Users_UserId",
                table: "EntryLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EntryLikes",
                table: "EntryLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "EntryLikes",
                newName: "EntryLike");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_EntryLikes_UserId",
                table: "EntryLike",
                newName: "IX_EntryLike_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EntryLikes_EntryId",
                table: "EntryLike",
                newName: "IX_EntryLike_EntryId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "Comment",
                newName: "IX_Comment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_EntryId",
                table: "Comment",
                newName: "IX_Comment_EntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntryLike",
                table: "EntryLike",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Entries_EntryId",
                table: "Comment",
                column: "EntryId",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Users_UserId",
                table: "Comment",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntryLike_Entries_EntryId",
                table: "EntryLike",
                column: "EntryId",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntryLike_Users_UserId",
                table: "EntryLike",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
