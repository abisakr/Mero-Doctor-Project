namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class GetDayAvailabilityDto
    {
        public int DoctorWeeklyAvailabilityId { get; set; }
        public string DayOfWeek { get; set; }
        public string AvailableDate { get; set; }
        public List<GetTimeRangeDto> TimeRanges { get; set; }
    }
}
