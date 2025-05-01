using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.Models
{
    public class DoctorWeeklyTimeRange
    {
        [Key]
        public int DoctorWeeklyTimeRangeId { get; set; } // Standardized property name

        [Required]
        public int DoctorWeeklyAvailabilityId { get; set; } // FK to DoctorWeeklyAvailability

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public DoctorWeeklyAvailability WeeklyAvailability { get; set; } // Navigation property

    }
}
