using Mero_Doctor_Project.DTOs.DoctorDto;
using Mero_Doctor_Project.Models.Common;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IDoctorWeeklyAvailabilityRepository
    {
        Task<ResponseModel<string>> SetDoctorAvailabilityAsync(SetDoctorAvailabilityDto dto,string userId);
        Task<ResponseModel<GetDoctorAvailabilityDto>> GetDoctorAvailabilityAsync(string doctorId);
        Task<ResponseModel<string>> DeleteDoctorTimeRangeAsync(DeleteTimeRangeDto dto, string userId);
        Task<ResponseModel<string>> DeleteDoctorDayAvailabililtyAsync(DeleteWeekdayDto dto, string userId);
    }
}
