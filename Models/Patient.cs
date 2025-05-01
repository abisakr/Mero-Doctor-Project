using Mero_Doctor_Project.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.Models
{
    public class Patient
    {
        [Key]
     public int PatientId { get; set; }

    [Required]
    public string UserId { get; set; } // FK to AspNetUsers.Id

    public DateTime DateOfBirth { get; set; }


    public Gender Gender { get; set; }

    [StringLength(100)]
    public string Address { get; set; }

    [Required]
    public double Latitude { get; set; } // Required for location
    [Required]
    public double Longitude { get; set; } // Required for location

    public ApplicationUser User { get; set; } // Navigation property
    public ICollection<Appointment> Appointments { get; set; } // Navigation property for appointments
    public ICollection<XRayRecord> XRayRecords { get; set; }
    }
}
