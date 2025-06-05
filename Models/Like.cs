using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.Models
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; } // Standardized property name

        [Required]
        public int BlogId { get; set; }

        public string UserId { get; set; }
        [Required]
        public string Name { get; set; } 

        public DateTime LikedDate { get; set; }

        public Blog Blog { get; set; } // Navigation property
        public ApplicationUser? User { get; set; } // Navigation property (nullable)

    }
}
