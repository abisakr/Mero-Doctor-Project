using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.Models
{
    public class RatingReview
    {
        [Key]
        public int RatingReviewId { get; set; } // Standardized property name

        [Required]
        public string UserId { get; set; } // FK to AspNetUsers.Id

        [Required]
        public int DoctorId { get; set; } // FK to Doctor

        [Range(1, 5)] // Rating between 1 and 5
        public int Rating { get; set; }
        [StringLength(500)]
        public string? Review { get; set; } // Can be null
        public DateTime CreatedDate { get; set; }
        public ApplicationUser User { get; set; } // Navigation property to user
        public Doctor Doctor { get; set; } // Navigation property to doctor

    }
}
