using static Mero_Doctor_Project.Repositories.DoctorWeeklyAvailabilityRepository;

namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class SetDayAvailabilityDto
    {
        public DayOfWeek DayOfWeek { get; set; }
        public List<TimeRangeDto> TimeRanges { get; set; }
    }
}
