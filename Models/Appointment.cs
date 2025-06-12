using System.ComponentModel.DataAnnotations;
using Mero_Doctor_Project.Models.Enums;

namespace Mero_Doctor_Project.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; } // Standardized property name

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int PatientId { get; set; }

        public AppointmentStatus Status { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateTime DateTime { get; set; } // BookingDateTimeof the appointment
        //public DateTime BookingDateTime { get; set; } = DateTime.UtcNow;
        public decimal Price { get; set; } // e.g. 100
        public string? TransactionId { get; set; }
        public string? TransactionStatus { get; set; } // Pending, Complete, Failed
        public DateTime? PaymentDate { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

    }
}
