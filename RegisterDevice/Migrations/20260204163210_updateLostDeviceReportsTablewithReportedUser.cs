using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegisterDevice.Migrations
{
    /// <inheritdoc />
    public partial class updateLostDeviceReportsTablewithReportedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReportedByUserId",
                table: "LostDevicesReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportedByUserId",
                table: "LostDevicesReports");
        }
    }
}
