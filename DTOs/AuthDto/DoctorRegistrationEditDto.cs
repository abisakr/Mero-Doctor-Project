using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.AuthDto
{
    public class DoctorRegistrationEditDto
    {
        [StringLength(25)]
        public string FullName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [StringLength(400)]
        public string Degree { get; set; }

        [Range(0, 70)]
        public double Experience { get; set; }

        [StringLength(50)]
        public string RegistrationId { get; set; }

        [StringLength(400)]
        public string ClinicAddress { get; set; }

        public int SpecializationId { get; set; }
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
