using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ocasAssignment.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSignUps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSignUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeSignUps_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeSignUps_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Id_EmailAddress",
                table: "Employees",
                columns: new[] { "Id", "EmailAddress" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSignUps_EmployeeId",
                table: "EmployeeSignUps",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSignUps_EventId",
                table: "EmployeeSignUps",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSignUps_Id",
                table: "EmployeeSignUps",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_Id_Name",
                table: "Events",
                columns: new[] { "Id", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeSignUps");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
