using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgileRap_Process.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "statuses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "works",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderID = table.Column<int>(type: "int", nullable: true),
                    Project = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_works", x => x.ID);
                    table.ForeignKey(
                        name: "FK_works_statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "statuses",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "providers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_providers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_providers_users_UserID",
                        column: x => x.UserID,
                        principalTable: "users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_providers_works_WorkID",
                        column: x => x.WorkID,
                        principalTable: "works",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "workLog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkID = table.Column<int>(type: "int", nullable: true),
                    No = table.Column<int>(type: "int", nullable: true),
                    Project = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_workLog_statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "statuses",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_workLog_works_WorkID",
                        column: x => x.WorkID,
                        principalTable: "works",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "providersLog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkLogID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_providersLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_providersLog_users_UserID",
                        column: x => x.UserID,
                        principalTable: "users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_providersLog_workLog_WorkLogID",
                        column: x => x.WorkLogID,
                        principalTable: "workLog",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_providers_UserID",
                table: "providers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_providers_WorkID",
                table: "providers",
                column: "WorkID");

            migrationBuilder.CreateIndex(
                name: "IX_providersLog_UserID",
                table: "providersLog",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_providersLog_WorkLogID",
                table: "providersLog",
                column: "WorkLogID");

            migrationBuilder.CreateIndex(
                name: "IX_workLog_StatusID",
                table: "workLog",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_workLog_WorkID",
                table: "workLog",
                column: "WorkID");

            migrationBuilder.CreateIndex(
                name: "IX_works_StatusID",
                table: "works",
                column: "StatusID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "providers");

            migrationBuilder.DropTable(
                name: "providersLog");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "workLog");

            migrationBuilder.DropTable(
                name: "works");

            migrationBuilder.DropTable(
                name: "statuses");
        }
    }
}
