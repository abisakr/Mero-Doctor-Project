using static Mero_Doctor_Project.Repositories.DoctorWeeklyAvailabilityRepository;

namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class SetDayAvailabilityDto
    {
        public DateOnly AvailableDate { get; set; }
        public List<SetTimeRangeDto> TimeRanges { get; set; }
    }
}
