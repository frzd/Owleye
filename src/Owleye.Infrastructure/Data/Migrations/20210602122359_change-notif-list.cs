using Microsoft.EntityFrameworkCore.Migrations;

namespace Owleye.Infrastructure.Migrations
{
    public partial class changenotiflist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notification_EndPointId",
                table: "Notification");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_EndPointId",
                table: "Notification",
                column: "EndPointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notification_EndPointId",
                table: "Notification");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_EndPointId",
                table: "Notification",
                column: "EndPointId",
                unique: true);
        }
    }
}
