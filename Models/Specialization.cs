using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.Models
{
    public class Specialization
    {
        [Key]
        public int SpecializationId { get; set; } // Standardized property name

        [Required]
        [StringLength(20)]
        public string Name { get; set; } // E.g., "Cardiology", "Neurology", etc.

        public ICollection<Doctor> Doctors { get; set; } // Navigation property to doctors

    }
}
