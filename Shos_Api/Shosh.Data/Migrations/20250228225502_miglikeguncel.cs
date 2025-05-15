using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shosh.Data.Migrations
{
    /// <inheritdoc />
    public partial class miglikeguncel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Entries_EntryId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowedUserId",
                table: "UserFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowingUserId",
                table: "UserFollows");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Likes");

            migrationBuilder.RenameColumn(
                name: "FollowingUserId",
                table: "UserFollows",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "FollowedUserId",
                table: "UserFollows",
                newName: "FollowingId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollows_FollowingUserId",
                table: "UserFollows",
                newName: "IX_UserFollows_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollows_FollowedUserId",
                table: "UserFollows",
                newName: "IX_UserFollows_FollowingId");

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Likes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_CommentId",
                table: "Likes",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Comments_CommentId",
                table: "Likes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Entries_EntryId",
                table: "Likes",
                column: "EntryId",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowingId",
                table: "UserFollows",
                column: "FollowingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_AspNetUsers_UserId",
                table: "UserFollows",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Comments_CommentId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Entries_EntryId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowingId",
                table: "UserFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_AspNetUsers_UserId",
                table: "UserFollows");

            migrationBuilder.DropIndex(
                name: "IX_Likes_CommentId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Likes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserFollows",
                newName: "FollowingUserId");

            migrationBuilder.RenameColumn(
                name: "FollowingId",
                table: "UserFollows",
                newName: "FollowedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollows_UserId",
                table: "UserFollows",
                newName: "IX_UserFollows_FollowingUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollows_FollowingId",
                table: "UserFollows",
                newName: "IX_UserFollows_FollowedUserId");

            migrationBuilder.AddColumn<int>(
                name: "TargetId",
                table: "Likes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Likes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Entries_EntryId",
                table: "Likes",
                column: "EntryId",
                principalTable: "Entries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowedUserId",
                table: "UserFollows",
                column: "FollowedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowingUserId",
                table: "UserFollows",
                column: "FollowingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
