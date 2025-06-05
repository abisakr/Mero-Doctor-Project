using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.BlogsDto
{
    public class BlogCommentUpdateDto
    {
        [Required]
        public int BlogCommentId { get; set; }
        [Required]
        [StringLength(500)]
        public string Comment { get; set; }
    }
}
