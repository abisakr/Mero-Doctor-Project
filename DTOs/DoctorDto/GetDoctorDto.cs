using Mero_Doctor_Project.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class GetDoctorDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; } 
        public string RegistrationId { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DoctorStatus Status { get; set; }
        public string Degree { get; set; }
        public double Experience { get; set; }
        public string ClinicAddress { get; set; }
        public string SpecializationName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
