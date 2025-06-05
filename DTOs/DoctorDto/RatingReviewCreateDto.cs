namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class RatingReviewCreateDto
    {
        public int DoctorId { get; set; }
        public int Rating { get; set; }
        public string? Review { get; set; }
    }
}
