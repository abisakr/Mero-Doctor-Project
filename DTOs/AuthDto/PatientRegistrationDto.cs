using Mero_Doctor_Project.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.AuthDto
{
    public class PatientRegistrationDto
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
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        [MinLength(6)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "Password must be at least 6 characters long and contain at least one uppercase letter and one number.")]
        public string Password { get; set; }

    }
}
