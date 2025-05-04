using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.Specilization
{
    public class SpecializationDto
    {
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
    }
}
