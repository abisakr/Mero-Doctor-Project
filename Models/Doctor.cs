using Mero_Doctor_Project.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; } // Standardized property name

        [Required]
        public string UserId { get; set; } // FK to AspNetUsers.Id

        [Required]
        [StringLength(50)]
        public string RegistrationId { get; set; } // Renamed to RegistrationId for clarity

        public DoctorStatus Status { get; set; }

        [StringLength(40)]
        public string Degree { get; set; }

        [Range(0, 70)]
        public double Experience { get; set; }

        [StringLength(100)]
        public string ClinicAddress { get; set; }

        public int SpecializationId { get; set; }

        public ApplicationUser User { get; set; } // Navigation property
        public Specialization Specialization { get; set; } // Navigation property for Specialization
        public ICollection<RatingReview> Ratings { get; set; } // Navigation property for ratings
        public ICollection<DoctorWeeklyAvailability> WeeklyAvailabilities { get; set; } // Navigation property for weekly availability
        public ICollection<Blog> Blogs { get; set; }
        public ICollection<Appointment> Appointments { get; set; }


    }
}
