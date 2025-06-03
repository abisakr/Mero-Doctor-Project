using static Mero_Doctor_Project.Repositories.DoctorWeeklyAvailabilityRepository;

namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class DoctorAvailabilityDto
    {
        public int DoctorId { get; set; }
         public List<DayAvailabilityDto> Availabilities { get; set; }
    }
}
