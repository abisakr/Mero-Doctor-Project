using Mero_Doctor_Project.DTOs.Specilization;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface ISpecializationRepository
    {
        Task<ResponseModel<List<SpecializationDto>>> GetAllAsync();
        Task<ResponseModel<SpecializationDto>> GetByIdAsync(int id);
        Task<ResponseModel<SpecializationDto>> AddAsync(SpecializationDto dto);
        Task<ResponseModel<SpecializationDto>> UpdateAsync(int id, SpecializationDto dto);
        Task<ResponseModel<bool>> DeleteAsync(int id);
    }
}
