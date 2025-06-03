using Mero_Doctor_Project.Models.Common;
using static Mero_Doctor_Project.Repositories.DoctorWeeklyAvailabilityRepository;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IDoctorWeeklyAvailabilityRepository
    {
        Task<ResponseModel<string>> SetDoctorAvailabilityAsync(DoctorAvailabilityDto dto);

    }
}
