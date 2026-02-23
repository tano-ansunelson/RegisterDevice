using System.ComponentModel.DataAnnotations;

namespace RegisterDevice.Models
{
    public class ResetPasswordViewModel
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


    }
}
