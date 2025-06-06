using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.FeedbackDto
{
    public class FeedbackCreateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
    }
}
