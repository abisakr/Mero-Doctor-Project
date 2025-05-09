using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.AuthDto
{
    public class DoctorLoginDto
    {
        [Required(ErrorMessage = "RegistrationId is required.")]
        [StringLength(50)]
        public string RegistrationId { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }
    }
}
