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
        public TimeOnly AvailableTime { get; set; }
        public DateOnly AvailableDate { get; set; }
        public decimal Price { get; set; } 
        public AppointmentStatus Status { get; set; }
        public DateTime BookingDateTime { get; set; } 
        public string? TransactionId { get; set; }
        public string? TransactionStatus { get; set; } // Pending, Complete, Failed
        public DateTime? PaymentDate { get; set; }
        public bool Visited { get; set; } 
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

    }
}
