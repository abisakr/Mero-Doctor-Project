using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; } // Standardized property name

        [Required]
        [StringLength(50)]
        public string Name { get; set; } // E.g., "Health", "Wellness"

        public ICollection<Blog> Blogs { get; set; } // Navigation property

    }
}
