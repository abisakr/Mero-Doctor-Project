using Mero_Doctor_Project.DTOs.AuthDto;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IAuthDoctorRegistrationRepository
    {
         Task<ResponseModel<string>> DoctorLoginAsync(DoctorLoginDto model);
         Task<ResponseModel<Doctor>> DoctorRegisterAsync(DoctorRegistrationDto model);
        Task<ResponseModel<Doctor>> EditDoctorProfile(DoctorRegistrationEditDto dto, string userId);
    }
}
