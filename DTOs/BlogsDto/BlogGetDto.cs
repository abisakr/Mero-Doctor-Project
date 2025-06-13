using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.BlogsDto
{
    public class BlogGetDto
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CategoryName { get; set; }   
        public string DoctorName { get; set; }    
        public string BlogPictureUrl { get; set; }
        public int TotalLikes { get; set; }
    }
}
