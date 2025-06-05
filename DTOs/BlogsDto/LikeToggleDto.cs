using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.BlogsDto
{
    public class LikeToggleDto
    {
        [Required]
        public int BlogId { get; set; }
      
    }
}
