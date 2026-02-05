namespace RegisterDevice.Models
{
    public class AdminDashboardViewModel
    {

            public int TotalDevices { get; set; }

            public int TotalUser { get; set; } 
            public int TotalLostReports { get; set; }
            public int ActiveLostReports { get; set; }
            public int ResolvedReports { get; set; }
        

        public List<LostDeviceReport> RecentLostDevices { get; set; }
        public List<RegisteredDevice> RecentDevies { get; set; }

        public List <ApplicationUser> RecentUser { get; set; }




}
}
