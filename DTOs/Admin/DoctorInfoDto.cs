using System.ComponentModel.DataAnnotations;
using Mero_Doctor_Project.Models.Enums;

namespace Mero_Doctor_Project.DTOs.Admin
{
    public class DoctorInfoDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Degree { get; set; }
        public double Experience { get; set; }
        public string RegistrationId { get; set; }
        public string ClinicAddress { get; set; }
        public string Specialization { get; set; }
        public DoctorStatus status{ get; set; }

    }
}
