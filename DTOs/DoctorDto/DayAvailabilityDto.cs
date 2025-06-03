using static Mero_Doctor_Project.Repositories.DoctorWeeklyAvailabilityRepository;

namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class DayAvailabilityDto
    {
        public DayOfWeek DayOfWeek { get; set; }
        public List<TimeRangeDto> TimeRanges { get; set; }
    }
}
