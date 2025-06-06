using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.BlogsDto
{
    public class BlogCategoryAddDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
