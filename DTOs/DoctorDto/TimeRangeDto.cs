namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class TimeRangeDto
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
