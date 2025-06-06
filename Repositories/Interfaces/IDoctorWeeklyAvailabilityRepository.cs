using Mero_Doctor_Project.DTOs.DoctorDto;
using Mero_Doctor_Project.Models.Common;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IDoctorWeeklyAvailabilityRepository
    {
        Task<ResponseModel<string>> SetDoctorAvailabilityAsync(DoctorAvailabilityDto dto);
        Task<ResponseModel<DoctorAvailabilityDto>> GetDoctorAvailabilityAsync(int doctorId);
        Task<ResponseModel<string>> DeleteDoctorTimeRangeAsync(DeleteTimeRangeDto dto, string userId);
        Task<ResponseModel<string>> DeleteDoctorWeekdayAsync(DeleteWeekdayDto dto, string userId);
    }
}
