using Mero_Doctor_Project.DTOs.Admin;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models.Enums;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        public Task<ResponseModel<UpdateDoctorInfoDto>> VerifyDoctorAsync(int id, DoctorStatus status);
        public Task<ResponseModel<List<GetDoctorInfoDto>>> GetVerifiedDoctorsAsync();
        public Task<ResponseModel<List<GetDoctorInfoDto>>> GetPendingDoctorsAsync();
        public Task<ResponseModel<List<GetDoctorInfoDto>>> GetRejectedDoctorsAsync();
        Task<ResponseModel<AdminDashboardViewDto>> DashboardView();
        Task<ResponseModel<GetAdminDto>> GetAdminById(string userId);
    }

}
