using Mero_Doctor_Project.DTOs.AuthDto;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IAuthDoctorRegistrationRepository
    {
        public Task<ResponseModel<Doctor>> DoctorRegisterAsync(DoctorRegistrationDto model);
    }
}
