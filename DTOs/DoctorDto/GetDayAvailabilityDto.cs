namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class GetDayAvailabilityDto
    {
        public string DayOfWeek { get; set; }
        public List<TimeRangeDto> TimeRanges { get; set; }
    }
}
