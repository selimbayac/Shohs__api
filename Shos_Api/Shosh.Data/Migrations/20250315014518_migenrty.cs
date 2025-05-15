using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shosh.Data.Migrations
{
    /// <inheritdoc />
    public partial class migenrty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Topics_TopicId",
                table: "Entries");

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "Entries",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Topics_TopicId",
                table: "Entries",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Topics_TopicId",
                table: "Entries");

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "Entries",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Topics_TopicId",
                table: "Entries",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id");
        }
    }
}
