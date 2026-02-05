using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegisterDevice.Migrations
{
    /// <inheritdoc />
    public partial class FixLostDeviceFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DevicesId",
                table: "LostDevicesReports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DevicesId",
                table: "LostDevicesReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
