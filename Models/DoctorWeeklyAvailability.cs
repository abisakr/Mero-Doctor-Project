using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.Models
{
    public class DoctorWeeklyAvailability
    {
        [Key]
        public int DoctorWeeklyAvailabilityId { get; set; } // Standardized property name

        [Required]
        public int DoctorId { get; set; }

        public DateOnly AvailableDate { get; set; }

        public ICollection<DoctorWeeklyTimeRange> TimeRanges { get; set; }=
        new List<DoctorWeeklyTimeRange>();

        public Doctor Doctor { get; set; } // Navigation property

    }
}
