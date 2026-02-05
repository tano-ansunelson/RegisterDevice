using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RegisterDevice.Models;

namespace RegisterDevice.Data
{
    public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser>

    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }

        public DbSet<RegisteredDevice> MyDevices { get; set; }
        public DbSet<DeviceCheckResult> DeviceCheckResults { get; set; }
        public DbSet<DeviceImage> DeviceImages { get; set; }
        public DbSet<LostDeviceReport> LostDevicesReports { get; set; }

    }
}
