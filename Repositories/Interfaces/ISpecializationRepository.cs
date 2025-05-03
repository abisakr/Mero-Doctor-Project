using Mero_Doctor_Project.DTOs.Specilization;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface ISpecializationRepository
    {
        Task<ResponseModel<List<Specialization>>> GetAllAsync();
        Task<ResponseModel<Specialization>> GetByIdAsync(int id);
        Task<ResponseModel<Specialization>> AddAsync(SpecializationDto dto);
        Task<ResponseModel<Specialization>> UpdateAsync(int id, SpecializationDto dto);
        Task<ResponseModel<bool>> DeleteAsync(int id);
    }
}
