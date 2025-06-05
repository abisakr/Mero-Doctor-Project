namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class RatingReviewGetDto
    {
        public int RatingReviewId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string? Review { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
