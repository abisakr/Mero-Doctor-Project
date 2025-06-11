namespace Mero_Doctor_Project.DTOs.PneumoniaDetectionDto
{
    public class XRayLiveHistoryDto
    {
        public string XRayImageUrl { get; set; }
        public string Result { get; set; }
        public string GradCamUrl { get; set; }
        public List<HospitalRecommendationDto> RecommendedHospitals { get; set; } = new List<HospitalRecommendationDto>();

    }
}
