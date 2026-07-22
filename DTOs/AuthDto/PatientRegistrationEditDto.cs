using Mero_Doctor_Project.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.AuthDto
{
    public class PatientRegistrationEditDto
    {
        [StringLength(25)]
        public string FullName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
