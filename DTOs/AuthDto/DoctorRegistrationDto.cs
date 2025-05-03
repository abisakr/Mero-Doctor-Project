using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.AuthDto
{
    public class DoctorRegistrationDto
    {
        [Required]
        [StringLength(25)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(40)]
        public string Degree { get; set; }

        [Required]
        [Range(0, 70)]
        public double Experience { get; set; }

        [Required]
        [StringLength(50)]
        public string RegistrationId { get; set; }

        [Required]
        [StringLength(100)]
        public string ClinicAddress { get; set; }

        [Required]
        public int SpecializationId { get; set; } //GETS FROM DCOTOR REGISTRATION UI FROM SPECILIZATION DROPDOWN

        [Required]
        [MinLength(6)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "Password must be at least 6 characters long and contain at least one uppercase letter and one number.")]
        public string Password { get; set; }

    }
}

