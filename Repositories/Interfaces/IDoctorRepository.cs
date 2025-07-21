using Mero_Doctor_Project.DTOs.DoctorDto;
using Mero_Doctor_Project.Models.Common;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        //find doctor by Doctorid 
        Task<ResponseModel<GetDoctorDto>> GetDoctorByIdAsync(string userId);
        Task<ResponseModel<List<GetDoctorDto>>> GetDoctorsByFilterAsync(int? specializationId, string? doctorName);
        Task<ResponseModel<List<GetDoctorDto>>> GetAllDoctorsAsync();
        Task<ResponseModel<List<GetDoctorDto>>> GetAllTopDoctorsAsync();

    }
}
