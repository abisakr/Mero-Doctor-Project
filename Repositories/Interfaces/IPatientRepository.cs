using Mero_Doctor_Project.DTOs.DoctorDto;
using Mero_Doctor_Project.DTOs.PatientDto;
using Mero_Doctor_Project.Models.Common;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<ResponseModel<GetPatientDto>> GetPatientByIdAsync(string userId);
        Task<ResponseModel<List<GetPatientDto>>> GetAllPatientsAsync();
    }
}
