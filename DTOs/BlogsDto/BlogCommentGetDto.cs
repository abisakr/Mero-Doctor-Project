namespace Mero_Doctor_Project.DTOs.BlogsDto
{
    public class BlogCommentGetDto
    {
        public int BlogCommentId { get; set; }
        public int BlogId { get; set; }
        public string Comment { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
