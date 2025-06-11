namespace Mero_Doctor_Project.DTOs.PneumoniaDetectionDto
{
    public class HospitalRecommendationDto
    {
        public string Hospital { get; set; }
        public string Province { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Distance_km { get; set; }
    }
}
