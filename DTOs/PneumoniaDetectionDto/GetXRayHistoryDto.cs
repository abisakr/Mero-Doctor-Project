namespace Mero_Doctor_Project.DTOs.PneumoniaDetectionDto
{
    public class GetXRayHistoryDto
    {
        public string XRayImageUrl { get; set; }
        public string Result { get; set; }
        public string GradCamUrl { get; set; }
        public string RecommendedHospital { get; set; }
        public string DateTime { get; set; }
    }

}
