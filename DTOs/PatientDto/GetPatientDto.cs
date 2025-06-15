using Mero_Doctor_Project.Models.Enums;

namespace Mero_Doctor_Project.DTOs.PatientDto
{
    public class GetPatientDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
