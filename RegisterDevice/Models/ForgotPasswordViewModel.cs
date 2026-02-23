using System.ComponentModel.DataAnnotations;

namespace RegisterDevice.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }



    }
}
