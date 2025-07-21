using Mero_Doctor_Project.DTOs.AppointmentDto;
using Mero_Doctor_Project.DTOs.AuthDto;
using Mero_Doctor_Project.Models.Common;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<ResponseModel<AppointmentBookingResponseDto>> BookAppointmentAsync(BookAppointmentDto dto, string patientUserId);
        Task<ResponseModel<string>> UpdateAppointmentStatusAsync(UpdateAppointmentStatusDto dto, string doctorUserId);
        Task<ResponseModel<List<GetAppointmentDto>>> GetAppointmentsByDoctorAsync(string doctorUserId);
        Task<ResponseModel<List<GetAppointmentDto>>> GetTodaysDoctorAppontmentsAsync(string doctorUserId);
        Task<ResponseModel<List<GetAppointmentDto>>> GetTodaysPatientAppontmentsAsync(string patientUserId);
        Task<ResponseModel<List<GetAppointmentDto>>> GetAppointmentsByPatientAsync(string patientUserId);
        Task<ResponseModel<List<GetAppointmentDto>>> GetAllUpcomingAppointmentsAsync();

    }
}

