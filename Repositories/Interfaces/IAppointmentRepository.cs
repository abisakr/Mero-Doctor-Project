using Mero_Doctor_Project.DTOs.AppointmentDto;
using Mero_Doctor_Project.DTOs.AuthDto;
using Mero_Doctor_Project.Models.Common;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<ResponseModel<string>> BookAppointmentAsync(BookAppointmentDto dto, string userId);
        Task<ResponseModel<string>> UpdateAppointmentStatusAsync(UpdateAppointmentStatusDto dto, string doctorUserId);
        Task<ResponseModel<List<AppointmentDto>>> GetAppointmentsByDoctorAsync(int doctorId);
        Task<ResponseModel<List<AppointmentDto>>> GetAppointmentsByPatientAsync(string userId);
    }
}
