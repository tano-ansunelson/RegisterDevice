using System.ComponentModel.DataAnnotations;

namespace RegisterDevice.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name ="Full Name")]
        public string FullName { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }




    }
}
