using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.BlogsDto
{
    public class BlogCommentAddDto
    {
        [Required]
        public int BlogId { get; set; }
        [Required]
        [StringLength(500)]
        public string Comment { get; set; }
    }
}
