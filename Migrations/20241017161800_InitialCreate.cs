using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG_POE2.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    ClaimID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LecturerFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LecturerLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoursWorked = table.Column<int>(type: "int", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClaimStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClaimEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "[HoursWorked] * [HourlyRate]", stored: true),
                    SupportingDocument = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    SupportingDocumentExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.ClaimID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Claims");
        }
    }
}
