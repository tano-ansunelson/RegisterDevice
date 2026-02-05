using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace RegisterDevice.Models
{
    public class RegisteredDevice
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string DeviceType { get; set; } // e.g., "Phone" or "Laptop"

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }
    

        [Required]
        public string Identifier { get; set; } // IMEI or Serial Number

        [Required]
        public string Status { get; set; } = "Active"; // Active, Lost, etc.

        public string Notes { get; set; } // Optional extra information

      public ICollection<DeviceImage> DeviceImages { get; set; }= new List<DeviceImage>();


        public string? UserId { get; set; }
        
        [ValidateNever]
        public ApplicationUser? User { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        // Optional: UserId if you implement authentication
        // public string UserId { get; set; }

    }
}
