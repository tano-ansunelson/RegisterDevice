namespace RegisterDevice.Models
{
    public class DeviceImage
    {
        public int Id { get; set; }

        public int RegisteredDeviceId { get; set; }
        public RegisteredDevice RegisteredDevice { get; set; }

        public string ImagePath { get; set; }

        public DateTime UploadedAt { get; set; }

    }
}
