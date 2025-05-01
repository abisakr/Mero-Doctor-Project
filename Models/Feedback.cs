using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; } // Standardized property name

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
