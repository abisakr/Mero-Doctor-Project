using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.BlogsDto
{
    public class BlogCategoryUpdateDto
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
