using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.BlogsDto
{
    public class BlogUpdateDto
    {
        [Required]
        public int BlogId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
