using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.DTOs.BlogsDto
{
    public class BlogGetDto
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CategoryName { get; set; }      // from Category.Name
        public string DoctorName { get; set; }        // from Doctor.User.FullName
        public int TotalLikes { get; set; }
    }
}
