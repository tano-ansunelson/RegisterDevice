namespace RegisterDevice.Models
{
    public class DeviceCheckResult
    {

        public int Id { get; set; }

        public string IMEI { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string DeviceName { get; set; } // e.g., Moto G22

        public string Status { get; set; } = "Unknown"; // Active, Lost, etc.

        public DateTime CheckedAt { get; set; } = DateTime.Now;

    }
}
