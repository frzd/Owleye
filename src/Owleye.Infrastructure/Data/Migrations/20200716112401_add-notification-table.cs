using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Owleye.Infrastructure.Migrations
{
    public partial class addnotificationtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Modified = table.Column<DateTimeOffset>(nullable: true),
                    CreatedById = table.Column<int>(nullable: false),
                    ModifiedById = table.Column<int>(nullable: true),
                    EndPointId = table.Column<int>(nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_EndPoints_EndPointId",
                        column: x => x.EndPointId,
                        principalTable: "EndPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_EndPointId",
                table: "Notification",
                column: "EndPointId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");
        }
    }
}
