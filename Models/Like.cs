using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.Models
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; } // Standardized property name

        [Required]
        public int BlogId { get; set; }

        public string? UserId { get; set; } // Nullable for guest likes
        [Required]
        public string Name { get; set; } //if guest then set name guest if userloggd in then put username  

        public DateTime LikedDate { get; set; }

        public Blog Blog { get; set; } // Navigation property
        public ApplicationUser? User { get; set; } // Navigation property (nullable)

    }
}
