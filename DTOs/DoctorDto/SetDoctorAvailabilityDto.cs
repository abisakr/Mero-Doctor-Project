using static Mero_Doctor_Project.Repositories.DoctorWeeklyAvailabilityRepository;

namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class SetDoctorAvailabilityDto
    {
         public List<SetDayAvailabilityDto> Availabilities { get; set; }
    }
}
