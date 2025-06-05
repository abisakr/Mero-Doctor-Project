namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class DoctorRatingSummaryDto
    {
        public int AverageRating { get; set; } 
        public List<RatingReviewGetDto> Reviews { get; set; }
    }
}
