using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace RegisterDevice.Models
{
    public class ApplicationUser:IdentityUser
    {

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }

        //[ValidateNever]
        //public string usersname { get; set; }

        //[Phone]
        //public string PhoneNumber { get; set; }

        // Navigation property
        [ValidateNever]
        public ICollection<RegisteredDevice> MyDevices { get; set; } = new List<RegisteredDevice>();


    }
}
