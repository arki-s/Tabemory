using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabemory.Data.Migrations
{
    public partial class dbcontextChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Record_RecordId",
                table: "Review");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Record_RecordId",
                table: "Review",
                column: "RecordId",
                principalTable: "Record",
                principalColumn: "RecordId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Record_RecordId",
                table: "Review");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Record_RecordId",
                table: "Review",
                column: "RecordId",
                principalTable: "Record",
                principalColumn: "RecordId");
        }
    }
}
