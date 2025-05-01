using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.Models
{
    public class XRayRecord
    {
        [Key]
        public int XRayRecordId { get; set; } // Standardized property name

        [Required]
        public int PatientId { get; set; }

        [Required]
        [Url]
        public string XRayImageUrl { get; set; }

        [Required]
        [StringLength(50)]
        public string Result { get; set; } // Positive/Negative

        [StringLength(200)]
        public string RecommendedHospital { get; set; }

        [Url]
        public string? GradCamUrl { get; set; }

        public DateTime DateTime { get; set; }

        public Patient Patient { get; set; } // Navigation property

    }
}
