using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.Specilization;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly ApplicationDbContext _context;
        public SpecializationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<Specialization>>> GetAllAsync()
        {
            try
            {
                var specializations = await _context.Specializations.ToListAsync();
                return new ResponseModel<List<Specialization>> { Success = true, Message = "Fetched all", Data = specializations };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<Specialization>> { Success = false, Message = $"Error: {ex.Message}", Data = null };
            }
        }

        public async Task<ResponseModel<Specialization>> GetByIdAsync(int id)
        {
            try
            {
                var specialization = await _context.Specializations.FindAsync(id);
                if (specialization == null)
                    return new ResponseModel<Specialization> { Success = false, Message = "Not found", Data = null };

                return new ResponseModel<Specialization> { Success = true, Message = "Found", Data = specialization };
            }
            catch (Exception ex)
            {
                return new ResponseModel<Specialization> { Success = false, Message = $"Error: {ex.Message}", Data = null };
            }
        }

        public async Task<ResponseModel<Specialization>> AddAsync(SpecializationDto dto)
        {
            try
            {
                var specialization = new Specialization { Name = dto.Name };
                await _context.Specializations.AddAsync(specialization);
                await _context.SaveChangesAsync();

                return new ResponseModel<Specialization> { Success = true, Message = "Created", Data = specialization };
            }
            catch (Exception ex)
            {
                return new ResponseModel<Specialization> { Success = false, Message = $"Error: {ex.Message}", Data = null };
            }
        }

        public async Task<ResponseModel<Specialization>> UpdateAsync(int id, SpecializationDto dto)
        {
            try
            {
                var specialization = await _context.Specializations.FindAsync(id);
                if (specialization == null)
                    return new ResponseModel<Specialization> { Success = false, Message = "Not found", Data = null };

                specialization.Name = dto.Name;
                await _context.SaveChangesAsync();

                return new ResponseModel<Specialization> { Success = true, Message = "Updated", Data = specialization };
            }
            catch (Exception ex)
            {
                return new ResponseModel<Specialization> { Success = false, Message = $"Error: {ex.Message}", Data = null };
            }
        }

        public async Task<ResponseModel<bool>> DeleteAsync(int id)
        {
            try
            {
                var specialization = await _context.Specializations.FindAsync(id);
                if (specialization == null)
                    return new ResponseModel<bool> { Success = false, Message = "Not found", Data = false };

                _context.Specializations.Remove(specialization);
                await _context.SaveChangesAsync();

                return new ResponseModel<bool> { Success = true, Message = "Deleted", Data = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool> { Success = false, Message = $"Error: {ex.Message}", Data = false };
            }
        }
    }
}
