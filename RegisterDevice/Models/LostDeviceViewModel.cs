namespace RegisterDevice.Models
{
    public class LostDeviceViewModel
    {
        public string Brand { get; set; }
        public string Model { get; set; }

        public string Identifier { get; set;  }

        public string OwnerName { get; set; }
        public DateTime ReportedAt { get; set; }

    }
}
