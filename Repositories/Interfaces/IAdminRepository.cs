using Mero_Doctor_Project.DTOs.Admin;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models.Enums;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        public Task<ResponseModel<DoctorInfoDto>> VerifyDoctorAsync(int id, DoctorStatus status);
        public Task<ResponseModel<List<DoctorInfoDto>>> GetVerifiedDoctorsAsync();
        public Task<ResponseModel<List<DoctorInfoDto>>> GetPendingDoctorsAsync();
        public Task<ResponseModel<List<DoctorInfoDto>>> GetRejectedDoctorsAsync();
    }

}
