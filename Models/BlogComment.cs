using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.Models
{
    public class BlogComment
    {
        [Key]
        public int BlogCommentId { get; set; } // Standardized property name

        [Required]
        public int BlogId { get; set; }

        public string? UserId { get; set; } // Nullable for guest comments
        [Required]
        public string Name { get; set; } //if guest then set name guest if userloggd in then put username

        [Required]
        [StringLength(500)]
        public string Comment { get; set; }

        public DateTime CreatedDate { get; set; }

        public Blog Blog { get; set; } // Navigation property
        public ApplicationUser? User { get; set; } // Navigation property for user (can be null for guest)

    }
}
