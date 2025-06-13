using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.Models
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; } // Standardized property name

        [Required]
        public int DoctorId { get; set; } // FK to Doctor

        [Required]
        public int CategoryId { get; set; } // FK to Category

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        [Url]
        public string BlogPictureUrl { get; set; }
        public DateTime CreatedDate { get; set; }

        public Doctor Doctor { get; set; } // Navigation property
        public Category Category { get; set; } // Navigation property
        public ICollection<BlogComment> BlogComments { get; set; } 
        public ICollection<Like> Likes { get; set; }
    }
}
