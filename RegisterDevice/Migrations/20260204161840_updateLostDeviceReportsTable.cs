using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegisterDevice.Migrations
{
    /// <inheritdoc />
    public partial class updateLostDeviceReportsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoseDevices_MyDevices_DeviceId",
                table: "LoseDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoseDevices",
                table: "LoseDevices");

            migrationBuilder.RenameTable(
                name: "LoseDevices",
                newName: "LostDevicesReports");

            migrationBuilder.RenameIndex(
                name: "IX_LoseDevices_DeviceId",
                table: "LostDevicesReports",
                newName: "IX_LostDevicesReports_DeviceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LostDevicesReports",
                table: "LostDevicesReports",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LostDevicesReports_MyDevices_DeviceId",
                table: "LostDevicesReports",
                column: "DeviceId",
                principalTable: "MyDevices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LostDevicesReports_MyDevices_DeviceId",
                table: "LostDevicesReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LostDevicesReports",
                table: "LostDevicesReports");

            migrationBuilder.RenameTable(
                name: "LostDevicesReports",
                newName: "LoseDevices");

            migrationBuilder.RenameIndex(
                name: "IX_LostDevicesReports_DeviceId",
                table: "LoseDevices",
                newName: "IX_LoseDevices_DeviceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoseDevices",
                table: "LoseDevices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoseDevices_MyDevices_DeviceId",
                table: "LoseDevices",
                column: "DeviceId",
                principalTable: "MyDevices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
