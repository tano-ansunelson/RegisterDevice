using System.Diagnostics.Contracts;

namespace RegisterDevice.Models
{
    public class LostDeviceReport
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }
        public RegisteredDevice Device { get; set; }

        public string Brand { get; set; }
        public string Model { get; set; }
        public string Identifier { get; set; }

        public string ReportedByUserId { get; set; }

        public DateTime ReportedAt { get; set; }
        public bool IsResolved { get; set; } //found or not




    }
}
